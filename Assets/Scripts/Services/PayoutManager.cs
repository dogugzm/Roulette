using System.Linq;
using UnityEngine;

public class PayoutManager : IPayoutManager
{
    private readonly IBettingManager _bettingManager;

    public PayoutManager(IBettingManager bettingManager)
    {
        _bettingManager = bettingManager;
    }

    public void CalculatePayouts(int winningNumber)
    {
        var bets = _bettingManager.GetCurrentBets();
        int totalWinnings = 0;

        foreach (var bet in bets)
        {
            if (IsWinner(bet, winningNumber))
            {
                int payout = BetRules.GetPayout(bet.BetType);
                int winnings = bet.Amount + (bet.Amount * payout);
                totalWinnings += winnings;
            }
        }

        if (totalWinnings > 0)
        {
            _bettingManager.AwardWinnings(totalWinnings);
        }

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