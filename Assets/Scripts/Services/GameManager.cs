using System.Collections.Generic;
using System.Threading.Tasks;
using DI;
using Models;
using Services.Interfaces;
using UI;
using UnityEngine;
using Wheel;

namespace Services
{
    public class GameManager : MonoBehaviour
    {
        private const int PAYOUT_DELAY_TIME = 4000;

        public enum GameState
        {
            Idle,
            Betting,
            Spinning,
            Payout
        }

        [SerializeField] private DeterministicUI deterministicUI;
        private GameState _currentState;

        private IBettingManager _bettingManager;
        private IPayoutManager _payoutManager;
        private IWheelController _wheelController;
        private IStatisticService _statisticService;
        private ICameraService _cameraService;
        private ISaveLoadService _saveLoadService;
        private IAudioManager _audioManager;
        private IChipManager _chipManager;

        void Start()
        {
            _bettingManager = ServiceLocator.Get<IBettingManager>();
            _payoutManager = ServiceLocator.Get<IPayoutManager>();
            _wheelController = ServiceLocator.Get<IWheelController>();
            _statisticService = ServiceLocator.Get<IStatisticService>();
            _cameraService = ServiceLocator.Get<ICameraService>();
            _saveLoadService = ServiceLocator.Get<ISaveLoadService>();
            _audioManager = ServiceLocator.Get<IAudioManager>();
            _chipManager = ServiceLocator.Get<IChipManager>();

            _audioManager.PlaySound(SFXConstants.BackgroundMusic, 0.2f);
            _wheelController.OnSpinComplete += OnWheelSpinComplete;
            _bettingManager.OnBetsPlaced += BettingManagerOnOnBetsPlaced;

            LoadGame();
            _ = ChangeState(GameState.Betting);
        }

        private void BettingManagerOnOnBetsPlaced()
        {
            SaveGame();
        }

        private void OnDestroy()
        {
            if (_wheelController != null)
            {
                _wheelController.OnSpinComplete -= OnWheelSpinComplete;
            }
        }

        private async Task ChangeState(GameState newState)
        {
            if (_currentState == newState) return;

            _currentState = newState;
            await _cameraService.MoveToTransformAsync(_currentState, destroyCancellationToken);
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

            _wheelController.Spin(deterministicUI.SelectedDeterministicValue);
        }

        private async void OnWheelSpinComplete(int winningNumber)
        {
            Debug.Log($"Wheel stopped. Winning number is: {winningNumber}");
            await PayoutRoutine(winningNumber);
            SaveGame();
        }

        private async Task PayoutRoutine(int winningNumber)
        {
            await ChangeState(GameState.Payout);
            Debug.Log("Displaying results and paying out winnings...");
            _statisticService.RecordWinningNumber(winningNumber);
            _payoutManager.CalculatePayouts(winningNumber);
            Debug.Log("Payouts complete. Clearing bets for the next round.");
            await Task.Delay(PAYOUT_DELAY_TIME);
            _bettingManager.ClearBets();
            await ChangeState(GameState.Betting);
        }

        public void SaveGame()
        {
            var statisticData = new Models.StatisticData
            {
                TotalSpins = _statisticService.TotalSpins,
                TotalWins = _statisticService.TotalWins,
                TotalLosses = _statisticService.TotalLosses,
                TotalProfitLoss = _statisticService.TotalProfitLoss,
                WinningNumbers = new List<int>(_statisticService.WinningNumbers)
            };

            var gameData = new GameData
            {
                PlayerBalance = _bettingManager.PlayerBalance,
                CurrentBets = new List<Bet>(_bettingManager.GetCurrentBets()),
                Statistics = statisticData,
                CurrentChipId = _chipManager.CurrentChipSo?.Id
            };

            _saveLoadService.Save("GameData", gameData);
            Debug.Log("Game saved!");
        }

        public void LoadGame()
        {
            if (!_saveLoadService.HasKey("GameData")) return;

            var gameData = _saveLoadService.Load<GameData>("GameData");

            _chipManager.RestoreState(gameData.CurrentChipId);
            _bettingManager.RestoreState(gameData.PlayerBalance, gameData.CurrentBets);
            _statisticService.RestoreState(gameData.Statistics);

            Debug.Log("Game loaded!");
        }
    }
}