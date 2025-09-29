using DefaultNamespace;
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
            if (_statisticService != null)
            {
                _statisticService.OnWinningNumberRecorded += AddWinningNumber;
            }
        }

        private void AddWinningNumber(int number)
        {
            var go = Instantiate(numberPrefab, content);
            go.Init(number);
        }
    }
}