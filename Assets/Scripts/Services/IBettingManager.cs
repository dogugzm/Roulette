using System;
using System.Collections.Generic;

public interface IBettingManager
{
    event Action OnBetsCleared;
    int PlayerBalance { get; }
    bool PlaceBet(int amount, BetType betType, int[] numbers);
    void ClearBets();
    void AwardWinnings(int amount);
    IReadOnlyList<Bet> GetCurrentBets();
}