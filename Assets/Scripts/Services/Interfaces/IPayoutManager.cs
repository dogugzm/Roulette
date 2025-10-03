using System;
using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IPayoutManager
    {
        event Action<List<Bet>> OnWinningBets;
        Action OnPayoutCompleted { get; set; }
        void CalculatePayouts(int winningNumber);
    }
}