using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public interface IStatisticService
    {
        int TotalSpins { get; }
        int TotalWins { get; }
        int TotalLosses { get; }
        float TotalProfitLoss { get; }
        void RecordWinningNumber(int number);
        Action<int> OnWinningNumberRecorded { get; set; }
        Action SpinRecorded { get; set; }
        void RecordSpin(bool isWin, float amountWonOrLost);
        void Reset();
    }
}