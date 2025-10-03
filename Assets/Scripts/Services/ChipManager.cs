using System.Collections.Generic;
using JetBrains.Annotations;
using Models;
using ScriptableObject;
using Services.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Services
{
    public class ChipManager : IChipManager
    {
        private IBettingManager _bettingManager;
        private IAudioManager _audioManager;

        public ChipSO CurrentChipSo { get; set; }

        public void RestoreState(ChipSO currentChipSo)
        {
            CurrentChipSo = currentChipSo;
        }

        private List<GameObject> _placedChips = new();

        [CanBeNull]
        public GameObject TryPlaceChip(Transform parent)
        {
            if (CurrentChipSo == null) return null;
            _audioManager?.PlaySound(SFXConstants.ChipPut, Random.Range(0.8f, 1.2f));
            var chipInstance = Object.Instantiate(CurrentChipSo.ChipPrefab, parent);
            _placedChips.Add(chipInstance);
            return chipInstance;
        }


        public ChipManager(IBettingManager bettingManager, IAudioManager audioManager)
        {
            _bettingManager = bettingManager;
            _audioManager = audioManager;
            if (_bettingManager != null)
            {
                _bettingManager.OnBetsCleared += HandleBetsCleared;
            }
        }

        private void Dispose()
        {
            if (_bettingManager != null)
            {
                _bettingManager.OnBetsCleared -= HandleBetsCleared;
            }
        }

        private void HandleBetsCleared()
        {
            foreach (var chip in _placedChips)
            {
                Object.Destroy(chip);
            }

            _placedChips.Clear();
        }
    }
}