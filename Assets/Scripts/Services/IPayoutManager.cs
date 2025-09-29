
using System;
using System.Collections.Generic;

public interface IPayoutManager
{
    event Action<List<Bet>> OnWinningBets;
    void CalculatePayouts(int winningNumber);
}
