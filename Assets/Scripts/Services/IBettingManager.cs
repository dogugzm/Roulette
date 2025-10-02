using System;
using System.Collections.Generic;

namespace Services
{
    public interface IBettingManager
    {
        event Action OnBetsCleared;
        int PlayerBalance { get; }
        bool TryPlaceBet(int amount, BetType betType, int[] numbers);
        void ClearBets();
        void AwardWinnings(int amount);
        IReadOnlyList<Bet> GetCurrentBets();
        void RestoreState(int balance, IReadOnlyList<Bet> bets);
        Action<int, IReadOnlyList<Bet>> OnRestoreCompleted { get; set; }
    }
}