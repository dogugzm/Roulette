using System.Collections.Generic;
using Services;
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

        private float chipYOffset = 0.3f;

        private IBettingManager _bettingManager;
        private IChipManager _chipManager;
        private Button _button;

        private int _chipAmount = 0;

        void Start()
        {
            _bettingManager = ServiceLocator.Get<IBettingManager>();
            _chipManager = ServiceLocator.Get<IChipManager>();

            _bettingManager.OnBetsCleared += () => _chipAmount = 0;

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnSpotClicked);
        }

        private void OnSpotClicked()
        {
            int chipValue = _chipManager.CurrentChipSo.Value;

            if (chipValue <= 0)
            {
                Debug.LogWarning("Please select a chip first!");
                return;
            }

            int[] betNumbers = (betType >= BetType.Red) ? null : numbers;

            bool success = _bettingManager.TryPlaceBet(chipValue, betType, betNumbers);

            if (success)
            {
                var chip = _chipManager.TryPlaceChip(transform);
                if (chip != null)
                {
                    _chipAmount++;
                    chip.transform.position += new Vector3(
                        Random.Range(-0.1f, 0.1f),
                        chipYOffset * _chipAmount,
                        Random.Range(-0.1f, 0.1f)
                    );
                }
            }
        }
    }
}