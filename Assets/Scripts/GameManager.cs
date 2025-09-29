using System.Collections.Generic;
using DI;
using System.Threading.Tasks;
using Services;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Idle,
        Betting,
        Spinning,
        Payout
    }

    private GameState _currentState;

    private IBettingManager _bettingManager;
    private IPayoutManager _payoutManager;
    private IWheelController _wheelController;
    private IStatisticService _statisticService;
    private ICameraService _cameraService;

    void Start()
    {
        _bettingManager = ServiceLocator.Get<IBettingManager>();
        _payoutManager = ServiceLocator.Get<IPayoutManager>();
        _wheelController = ServiceLocator.Get<IWheelController>();
        _statisticService = ServiceLocator.Get<IStatisticService>();
        _cameraService = ServiceLocator.Get<ICameraService>();

        _wheelController.OnSpinComplete += OnWheelSpinComplete;

        ChangeState(GameState.Betting);
    }

    private void OnDestroy()
    {
        if (_wheelController != null)
        {
            _wheelController.OnSpinComplete -= OnWheelSpinComplete;
        }
    }

    private void ChangeState(GameState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        _cameraService.MoveToTransformAsync(_currentState, destroyCancellationToken);
        Debug.Log($"--- New State: {_currentState} ---");
    }


    public void SpinButtonPressed()
    {
        if (_currentState == GameState.Betting)
        {
            StartSpin();
        }
        else
        {
            Debug.LogWarning($"Cannot spin in the current state: {_currentState}");
        }
    }

    private void StartSpin()
    {
        ChangeState(GameState.Spinning);
        Debug.Log("Spin button pressed. No more bets! Wheel is spinning...");

        // _bettingManager.LockBets();

        _wheelController.Spin();
    }

    private async void OnWheelSpinComplete(int winningNumber)
    {
        Debug.Log($"Wheel stopped. Winning number is: {winningNumber}");

        _statisticService.RecordWinningNumber(winningNumber);
        _payoutManager.CalculatePayouts(winningNumber);

        await PayoutRoutine();
    }

    private async Task PayoutRoutine()
    {
        ChangeState(GameState.Payout);
        Debug.Log("Displaying results and paying out winnings...");

        await Task.Delay(3000);

        Debug.Log("Payouts complete. Clearing bets for the next round.");
        _bettingManager.ClearBets();

        ChangeState(GameState.Betting);
    }
}