using System;
using System.Collections.Generic;

namespace Services
{
    public interface IPayoutManager
    {
        event Action<List<Bet>> OnWinningBets;
        void CalculatePayouts(int winningNumber);
    }
}
