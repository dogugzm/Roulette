using System.Collections.Generic;
using System.Threading.Tasks;
using DI;
using Helper;
using Services.Interfaces;
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

        [SerializeField] private GameObject clickEffect;

        private const float ChipYOffset = 0.3f;

        private IBettingManager _bettingManager;
        private IChipManager _chipManager;
        private Button _button;
        public BetType BetType => betType;
        public IReadOnlyList<int> Numbers => numbers;

        private int _chipAmount = 0;

        void Start()
        {
            _bettingManager = ServiceLocator.Get<IBettingManager>();
            _chipManager = ServiceLocator.Get<IChipManager>();

            _bettingManager.OnBetsCleared += () => _chipAmount = 0;
            _bettingManager.RegisterBettingSpot(this);

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnSpotClicked);
        }


        private void OnSpotClicked()
        {
            var currentChip = _chipManager.CurrentChipSo;
            if (currentChip == null || currentChip.Value <= 0)
            {
                Debug.LogWarning("Please select a chip first!");
                return;
            }

            int[] betNumbers = betType is >= BetType.Red and <= BetType.High ? null : numbers;

            var success = _bettingManager.TryPlaceBet(currentChip.Value, betType, betNumbers, currentChip.Id);
            _ = EffectBehavior();

            if (success)
            {
                PlaceChipVisual(currentChip.Id);
            }
        }

        public void PlaceChipVisual(string chipId)
        {
            var chip = _chipManager.PlaceChipById(chipId, transform);
            if (chip != null)
            {
                _chipAmount++;
                chip.transform.position += new Vector3(
                    Random.Range(-0.1f, 0.1f),
                    ChipYOffset * _chipAmount,
                    Random.Range(-0.1f, 0.1f)
                );
            }
        }

        private async Task EffectBehavior()
        {
            clickEffect.gameObject.SetActive(true);
            await Task.Delay(1000, destroyCancellationToken);
            clickEffect.gameObject.SetActive(false);
        }


    }
}