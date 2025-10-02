using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Services;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace UI
{
    interface IChipManager
    {
        GameObject TryPlaceChip(Transform parent);
        Chip CurrentChip { get; set; }
        void RestoreState(IReadOnlyList<Bet> bets);
    }

    public class ChipManager : IChipManager
    {
        private IBettingManager _bettingManager;
        private ISfxManager _sfxManager;

        public Chip CurrentChip { get; set; }

        public void RestoreState(IReadOnlyList<Bet> bets)
        {
        }

        private List<GameObject> _placedChips = new();

        [CanBeNull]
        public GameObject TryPlaceChip(Transform parent)
        {
            if (CurrentChip == null) return null;
            _sfxManager?.PlaySound(SFXConstants.ChipPut, Random.Range(0.8f, 1.2f));
            var chipInstance = Object.Instantiate(CurrentChip.ChipPrefab, parent);
            _placedChips.Add(chipInstance);
            return chipInstance;
        }


        public ChipManager(IBettingManager bettingManager, ISfxManager sfxManager)
        {
            _bettingManager = bettingManager;
            _sfxManager = sfxManager;
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