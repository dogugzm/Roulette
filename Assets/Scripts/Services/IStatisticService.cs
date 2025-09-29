using System;

namespace Services
{
    public interface IStatisticService
    {
        int TotalSpins { get; }
        int TotalWins { get; }
        int TotalLosses { get; }
        float TotalProfitLoss { get; }
        System.Collections.Generic.IReadOnlyList<int> WinningNumbers { get; }
        void RecordWinningNumber(int number);
        Action<int> OnWinningNumberRecorded { get; set; }
        Action SpinRecorded { get; set; }
        void RecordSpin(bool isWin, float amountWonOrLost);
        void Reset();
        void RestoreState(Models.StatisticData data);
        Action<Models.StatisticData> RestoredCompleted { get; set; }
    }
}