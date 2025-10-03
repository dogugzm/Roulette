using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helper;
using Models;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class PayoutManager : IPayoutManager
    {
        public event Action<List<Bet>> OnWinningBets;
        public Action OnPayoutCompleted { get; set; }
        private readonly IBettingManager _bettingManager;
        private readonly IStatisticService _statisticService;
        private readonly IAudioManager _audioManager;
        private readonly GameObject _winnerEffect;
        private readonly GameObject _loseEffect;

        public PayoutManager(IBettingManager bettingManager, IStatisticService statisticService,
            GameObject winnerEffect, IAudioManager audioManager, GameObject loseEffect)
        {
            _bettingManager = bettingManager;
            _statisticService = statisticService;
            _winnerEffect = winnerEffect;
            _audioManager = audioManager;
            _loseEffect = loseEffect;
        }

        public void CalculatePayouts(int winningNumber)
        {
            var bets = _bettingManager.GetCurrentBets();
            int totalWinnings = 0;
            int totalBetAmount = bets.Sum(b => b.Amount);
            List<Bet> winningBets = new List<Bet>();

            foreach (var bet in bets)
            {
                if (IsWinner(bet, winningNumber))
                {
                    int payout = BetRules.GetPayout(bet.BetType);
                    int winnings = bet.Amount + (bet.Amount * payout);
                    totalWinnings += winnings;
                    winningBets.Add(bet);
                }
            }

            if (totalWinnings > 0)
            {
                OnWinningBets?.Invoke(winningBets);
                _ = WinnerEffectAsync(_winnerEffect);
                _audioManager.PlaySound(SFXConstants.Success);
                _bettingManager.AwardWinnings(totalWinnings);
            }
            else
            {
                _ = WinnerEffectAsync(_loseEffect);
                _audioManager.PlaySound(SFXConstants.Lose);
            }

            var profit = totalWinnings - totalBetAmount;
            _statisticService.RecordSpin(profit > 0, profit);

            OnPayoutCompleted?.Invoke();

            Debug.Log($"Winning Number: {winningNumber}. Total Payout: {totalWinnings}");
        }

        private async Task WinnerEffectAsync(GameObject effect)
        {
            effect.SetActive(true);
            await Task.Delay(4000);
            effect.SetActive(false);
        }

        private bool IsWinner(Bet bet, int winningNumber)
        {
            if (winningNumber is 0 or 37)
            {
                if (bet.BetType >= BetType.Red && bet.BetType <= BetType.Column3)
                {
                    return false;
                }
            }

            if (bet.BetType <= BetType.SixLine)
            {
                return bet.Numbers.Contains(winningNumber);
            }

            int[] winningSet = BetRules.GetBetNumbers(bet.BetType);
            return winningSet.Contains(winningNumber);
        }
    }
}