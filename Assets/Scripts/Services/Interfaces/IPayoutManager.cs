using System;
using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IPayoutManager
    {
        event Action<List<Bet>> OnWinningBets;
        void CalculatePayouts(int winningNumber);
    }
}
