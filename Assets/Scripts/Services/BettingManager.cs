using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class BettingManager : IBettingManager
    {
        public event Action OnBetsCleared;

        private List<Bet> _currentBets = new List<Bet>();
        private int _playerBalance;

        public int PlayerBalance => _playerBalance;

        public BettingManager(int startingBalance)
        {
            _playerBalance = startingBalance;
        }

        public bool PlaceBet(int amount, BetType betType, int[] numbers)
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

        public IReadOnlyList<Bet> GetCurrentBets()
        {
            return _currentBets.AsReadOnly();
        }
    }
}