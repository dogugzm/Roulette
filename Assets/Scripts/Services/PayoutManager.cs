using System;
using System.Collections.Generic;
using DefaultNamespace;
using System.Linq;
using UnityEngine;

public class PayoutManager : IPayoutManager
{
    public event Action<List<Bet>> OnWinningBets;
    private readonly IBettingManager _bettingManager;
    private readonly IStatisticService _statisticService;

    public PayoutManager(IBettingManager bettingManager, IStatisticService statisticService)
    {
        _bettingManager = bettingManager;
        _statisticService = statisticService;
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

        OnWinningBets?.Invoke(winningBets);

        if (totalWinnings > 0)
        {
            _bettingManager.AwardWinnings(totalWinnings);
        }

        var profit = totalWinnings - totalBetAmount;
        _statisticService.RecordSpin(profit > 0, profit);

        Debug.Log($"Winning Number: {winningNumber}. Total Payout: {totalWinnings}");
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