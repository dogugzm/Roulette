using Models;
using Services;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatisticsUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text totalSpinsText;
        [SerializeField] private TMP_Text totalWinsText;
        [SerializeField] private TMP_Text totalLossesText;
        [SerializeField] private TMP_Text totalProfitLossText;

        private IStatisticService _statisticService;

        private void Start()
        {
            _statisticService = ServiceLocator.Get<IStatisticService>();
            _statisticService.SpinRecorded += UpdateUI;
            _statisticService.RestoredCompleted += RestoredCompleted;
            UpdateUI();
        }

        private void RestoredCompleted(StatisticData data)
        {
            UpdateUI();
        }

        private void OnDestroy()
        {
            if (_statisticService != null)
            {
                _statisticService.SpinRecorded -= UpdateUI;
                _statisticService.RestoredCompleted -= RestoredCompleted;
            }
        }

        private void UpdateUI()
        {
            if (_statisticService == null) return;

            totalSpinsText.text = $"Total Spins: {_statisticService.TotalSpins}";
            totalWinsText.text = $"Total Wins: {_statisticService.TotalWins}";
            totalLossesText.text = $"Total Losses: {_statisticService.TotalLosses}";
            totalProfitLossText.text = $"Profit/Loss: {_statisticService.TotalProfitLoss:F2}";
        }
    }
}