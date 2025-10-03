using System;
using System.Collections.Generic;
using System.Linq;
using DI;
using Models;
using Services;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class BettingUI : MonoBehaviour
    {
        [SerializeField] private Text playerBalanceText;
        [SerializeField] private List<ChipUIView> chipViews;
        [SerializeField] private GameManager gameManager;

        [SerializeField] private Button spinButton;
        [SerializeField] private List<NumberUIView> numberUIs;

        private IBettingManager _bettingManager;
        private IChipManager _chipManager;
        private IPayoutManager _payoutManager;
        private ChipUIView _selectedChipView;
    

        public void SpinButtonPressed()
        {
            if (gameManager != null)
            {
                gameManager.SpinButtonPressed();
            }
            else
            {
                Debug.LogError("GameManager reference is not set in BettingUI!");
            }
        }

        void Start()
        {
            _bettingManager = ServiceLocator.Get<IBettingManager>();
            _chipManager = ServiceLocator.Get<IChipManager>();
            _payoutManager = ServiceLocator.Get<IPayoutManager>();

            _payoutManager.OnWinningBets += OnWinningBets;
            _bettingManager.OnBetsCleared += OnBetsCleared;
            _bettingManager.OnRestoreCompleted += OnRestoreCompleted;
            spinButton.onClick.AddListener(SpinButtonPressed);

            foreach (var chipUIView in chipViews)
            {
                _ = chipUIView.InitAsync(new ChipUIView.Data()
                {
                    clickAction = () => SelectChip(chipUIView.ChipSo.Value)
                });
            }
        }

        private void OnRestoreCompleted(int balance, IReadOnlyList<Bet> bets)
        {
            //playerBalanceText.text = $"Balance: {balance}";
            foreach (var bet in bets)
            {
            }
        }

        private void OnDestroy()
        {
            spinButton.onClick.RemoveListener(SpinButtonPressed);
            _payoutManager.OnWinningBets -= OnWinningBets;
            _bettingManager.OnBetsCleared -= OnBetsCleared;
        }

        private void OnBetsCleared()
        {
            foreach (var numberUI in numberUIs)
            {
                numberUI.Reset();
            }
        }

        private void OnWinningBets(List<Bet> winningBets)
        {
            foreach (var bet in winningBets)
            {
                foreach (var number in bet.Numbers)
                {
                    var numberUI = numberUIs.FirstOrDefault(n => n.number == number);
                    if (numberUI != null)
                    {
                        numberUI.Highlight();
                    }
                }
            }
        }

        void Update()
        {
            // if (_bettingManager != null)
            // {
            //     playerBalanceText.text = $"Balance: {_bettingManager.PlayerBalance}";
            // }
        }

        private void SelectChip(int value)
        {
            _chipManager.CurrentChipSo = chipViews.FirstOrDefault(chipView => chipView.ChipSo.Value == value)?.ChipSo;
            foreach (var chipUIView in chipViews)
            {
                bool isSelected = chipUIView.ChipSo.Value == value;
                chipUIView.SetSelected(isSelected);
            }

            Debug.Log($"Selected chip: {value}");
        }
    }
}