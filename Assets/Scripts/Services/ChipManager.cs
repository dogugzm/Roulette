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
        private readonly ChipDatabaseSO _chipDatabase;

        public ChipSO CurrentChipSo { get; set; }

        public void RestoreState(string currentChipId)
        {
            if (string.IsNullOrEmpty(currentChipId))
            {
                CurrentChipSo = null;
                return;
            }

            CurrentChipSo = _chipDatabase.GetChipByID(currentChipId);
        }

        private List<GameObject> _placedChips = new();

        public GameObject TryPlaceChip(Transform parent)
        {
            if (CurrentChipSo == null) return null;
            return PlaceChipById(CurrentChipSo.Id, parent);
        }

        public GameObject PlaceChipById(string chipId, Transform parent)
        {
            var chipSo = _chipDatabase.GetChipByID(chipId);
            if (chipSo == null)
            {
                Debug.LogWarning($"Chip with id {chipId} not found in database");
                return null;
            }

            _audioManager?.PlaySound(SFXConstants.ChipPut, Random.Range(0.8f, 1.2f));
            var chipInstance = Object.Instantiate(chipSo.ChipPrefab, parent);
            _placedChips.Add(chipInstance);
            return chipInstance;
        }


        public ChipManager(IBettingManager bettingManager, IAudioManager audioManager, ChipDatabaseSO chipDatabase)
        {
            _bettingManager = bettingManager;
            _audioManager = audioManager;
            _chipDatabase = chipDatabase;
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