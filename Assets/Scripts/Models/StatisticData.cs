using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class StatisticData
    {
        public int TotalSpins;
        public int TotalWins;
        public int TotalLosses;
        public float TotalProfitLoss;
        public List<int> WinningNumbers;
    }
}