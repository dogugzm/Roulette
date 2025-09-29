using System;
using System.Collections.Generic;

namespace Services
{
    public class StatisticService : IStatisticService
    {
        private int _totalSpins;
        private int _totalWins;
        private int _totalLosses;
        private float _totalProfitLoss;

        public int TotalSpins => _totalSpins;
        public int TotalWins => _totalWins;
        public int TotalLosses => _totalLosses;
        public float TotalProfitLoss => _totalProfitLoss;
        public List<int> WinningNumbers { get; } = new();

        public void RecordWinningNumber(int number)
        {
            WinningNumbers.Add(number);
            OnWinningNumberRecorded?.Invoke(number);
        }

        public Action<int> OnWinningNumberRecorded { get; set; }

        public Action SpinRecorded { get; set; }

        public void RecordSpin(bool isWin, float amountWonOrLost)
        {
            _totalSpins++;
            if (isWin)
            {
                _totalWins++;
            }
            else
            {
                _totalLosses++;
            }

            _totalProfitLoss += amountWonOrLost;
            SpinRecorded?.Invoke();
        }


        public void Reset()
        {
            _totalSpins = 0;
            _totalWins = 0;
            _totalLosses = 0;
            _totalProfitLoss = 0f;
        }
    }
}