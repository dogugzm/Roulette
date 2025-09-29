using System;

namespace DefaultNamespace
{
    public interface IStatisticService
    {
        int TotalSpins { get; }
        int TotalWins { get; }
        int TotalLosses { get; }
        float TotalProfitLoss { get; }
        Action SpinRecorded { get; set; }
        void RecordSpin(bool isWin, float amountWonOrLost);
        void Reset();
    }
}