using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class BettingSpotUI : MonoBehaviour
    {
        [Header("Betting Spot Configuration")] [SerializeField]
        private BetType betType;

        [SerializeField] private int[] numbers;

        private IBettingManager _bettingManager;
        private IChipManager _chipManager;
        private Button _button;

        void Start()
        {
            _bettingManager = ServiceLocator.Get<IBettingManager>();
            _chipManager = ServiceLocator.Get<IChipManager>();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnSpotClicked);
        }

        private void OnSpotClicked()
        {
            int chipValue = _chipManager.CurrentChipValue;

            if (chipValue <= 0)
            {
                Debug.LogWarning("Please select a chip first!");
                return;
            }

            int[] betNumbers = (betType >= BetType.Red) ? null : numbers;

            bool success = _bettingManager.PlaceBet(chipValue, betType, betNumbers);

            if (success)
            {
                _chipManager.OnChipPlaced(transform);
            }
        }
    }
}