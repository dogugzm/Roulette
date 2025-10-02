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
    }

    public class ChipManager : IChipManager
    {
        private IBettingManager _bettingManager;
        private IChipManager _chipManagerImplementation;
        public Chip CurrentChip { get; set; }

        private List<GameObject> _placedChips = new();

        [CanBeNull]
        public GameObject TryPlaceChip(Transform parent)
        {
            if (CurrentChip == null) return null;

            var chipInstance = Object.Instantiate(CurrentChip.ChipPrefab, parent);
            _placedChips.Add(chipInstance);
            return chipInstance;
        }


        public ChipManager(IBettingManager bettingManager)
        {
            _bettingManager = bettingManager;
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