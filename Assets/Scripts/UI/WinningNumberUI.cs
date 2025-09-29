using Models;
using Services;
using UnityEngine;

namespace UI
{
    public class WinningNumberUI : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private WinningNumberView numberPrefab;

        private IStatisticService _statisticService;

        private void Start()
        {
            _statisticService = ServiceLocator.Get<IStatisticService>();
            _statisticService.RestoredCompleted += OnRestoredCompleted;
            if (_statisticService != null)
            {
                _statisticService.OnWinningNumberRecorded += AddWinningNumber;
            }
        }

        private void OnRestoredCompleted(StatisticData statisticData)
        {
            if (_statisticService == null) return;

            foreach (var number in statisticData.WinningNumbers)
            {
                AddWinningNumber(number);
            }
        }

        private void AddWinningNumber(int number)
        {
            var go = Instantiate(numberPrefab, content);
            go.Init(number);
        }
    }
}