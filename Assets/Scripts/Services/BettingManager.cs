using System;
using System.Collections.Generic;
using Helper;
using Models;
using Services.Interfaces;
using UI;
using UnityEngine;

namespace Services
{
    public class BettingManager : IBettingManager
    {
        public event Action OnBetsCleared;

        private List<Bet> _currentBets = new();
        private int _playerBalance;

        public int PlayerBalance => _playerBalance;

        List<BettingSpotUI> _registeredBettingSpots = new();

        public BettingManager(int startingBalance)
        {
            _playerBalance = startingBalance;
        }

        public bool TryPlaceBet(int amount, BetType betType, int[] numbers)
        {
            if (amount > _playerBalance)
            {
                Debug.LogWarning("Not enough balance to place the bet.");
                return false;
            }

            _playerBalance -= amount;
            Bet bet = new Bet(amount, betType, numbers);
            _currentBets.Add(bet);
            Debug.Log($"Placed bet of {bet.Amount} on {bet.BetType}. Current balance: {_playerBalance}");
            return true;
        }

        public void AwardWinnings(int amount)
        {
            _playerBalance += amount;
            Debug.Log($"Awarded {amount}. New balance: {_playerBalance}");
        }

        public void ClearBets()
        {
            _currentBets.Clear();
            OnBetsCleared?.Invoke();
        }

        public void RegisterBettingSpot(BettingSpotUI bettingSpotUI)
        {
            if (!_registeredBettingSpots.Contains(bettingSpotUI))
            {
                _registeredBettingSpots.Add(bettingSpotUI);
            }
        }

        public IReadOnlyList<Bet> GetCurrentBets()
        {
            return _currentBets.AsReadOnly();
        }

        public void RestoreState(int balance, IReadOnlyList<Bet> bets)
        {
            _playerBalance = balance;
            // update text via playerBalance

            if (bets != null)
            {
                // _currentBets = new List<Bet>(bets);
                // // Recreate visual chips on betting spots if needed
                // foreach (var bet in bets)
                // {
                //     foreach (var spot in _registeredBettingSpots)
                //     {
                //         if (spot.BetType == bet.BetType)
                //         {
                //             
                //             spot.BehaveSpotClick();
                //         }
                //     }
                // }
            }

            OnRestoreCompleted?.Invoke(_playerBalance, GetCurrentBets());
        }

        public Action<int, IReadOnlyList<Bet>> OnRestoreCompleted { get; set; }
    }
}