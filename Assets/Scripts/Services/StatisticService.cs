namespace DefaultNamespace
{
    public class StatisticService
    {
        private int _totalSpins;
        private int _totalWins;
        private int _totalLosses;
        private float _totalProfitLoss;

        public int TotalSpins => _totalSpins;
        public int TotalWins => _totalWins;
        public int TotalLosses => _totalLosses;
        public float TotalProfitLoss => _totalProfitLoss;

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