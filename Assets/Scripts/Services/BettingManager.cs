using System;
using System.Collections.Generic;
using System.Linq;
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
        public event Action OnBetsPlaced;

        private List<Bet> _currentBets = new();
        private int _playerBalance;

        public int PlayerBalance => _playerBalance;

        List<BettingSpotUI> _registeredBettingSpots = new();

        public BettingManager(int startingBalance)
        {
            _playerBalance = startingBalance;
        }

        public bool TryPlaceBet(int amount, BetType betType, int[] numbers, string chipId)
        {
            if (amount > _playerBalance)
            {
                Debug.LogWarning("Not enough balance to place the bet.");
                return false;
            }

            _playerBalance -= amount;
            Bet bet = new Bet(amount, betType, numbers, chipId);
            _currentBets.Add(bet);
            OnBetsPlaced?.Invoke();
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
            RestoreBalance(balance);
            RestoreBets(bets);
            RestoreBetVisuals(bets);
            OnRestoreCompleted?.Invoke(_playerBalance, GetCurrentBets());
        }

        private void RestoreBalance(int balance)
        {
            _playerBalance = balance;
        }

        private void RestoreBets(IReadOnlyList<Bet> bets)
        {
            _currentBets = bets != null ? new List<Bet>(bets) : new List<Bet>();
        }

        private void RestoreBetVisuals(IReadOnlyList<Bet> bets)
        {
            if (bets == null) return;

            foreach (var bet in bets)
            {
                foreach (var spot in _registeredBettingSpots)
                {
                    if (IsMatchingSpot(spot, bet))
                    {
                        spot.PlaceChipVisual(bet.ChipId);
                        break; // stop after first match
                    }
                }
            }
        }

        private bool IsMatchingSpot(BettingSpotUI spot, Bet bet)
        {
            if (spot.BetType != bet.BetType)
                return false;

            if (bet.BetType <= BetType.SixLine)
                return NumbersMatch(spot.Numbers, bet.Numbers);

            return true;
        }

        private bool NumbersMatch(IReadOnlyList<int> spotNumbers, int[] betNumbers)
        {
            if (betNumbers == null || spotNumbers == null || betNumbers.Length != spotNumbers.Count)
                return false;

            var sortedBet = betNumbers.OrderBy(n => n);
            var sortedSpot = spotNumbers.OrderBy(n => n);

            return sortedBet.SequenceEqual(sortedSpot);
        }

        public Action<int, IReadOnlyList<Bet>> OnRestoreCompleted { get; set; }
    }
}