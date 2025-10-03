using System.Collections.Generic;
using System.Threading.Tasks;
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

        private class PlacedChip
        {
            public string ChipId;
            public ChipInstance Instance;
        }

        private readonly Dictionary<string, IPoolService<ChipInstance>> _chipPools = new();

        public async Task InitializeAsync()
        {
            foreach (var chipSo in _chipDatabase.GetAllChips())
            {
                var pool = new PoolService<ChipInstance>();
                var chipPrefab = chipSo.ChipPrefab.GetComponent<ChipInstance>();
                if (chipPrefab != null)
                {
                    await pool.InitializeAsync(chipPrefab, 10); // Initial pool size of 10
                    _chipPools[chipSo.Id] = pool;
                }
                else
                {
                    Debug.LogError($"Chip prefab for {chipSo.Id} is missing ChipInstance component!");
                }
            }
        }

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

        private List<PlacedChip> _placedChips = new();

        public GameObject TryPlaceChip(Transform parent)
        {
            if (CurrentChipSo == null) return null;
            return PlaceChipById(CurrentChipSo.Id, parent);
        }

        public GameObject PlaceChipById(string chipId, Transform parent)
        {
            if (!_chipPools.TryGetValue(chipId, out var pool))
            {
                Debug.LogError($"No pool found for chip id {chipId}");
                return null;
            }

            _audioManager?.PlaySound(SFXConstants.ChipPut, Random.Range(0.8f, 1.2f));
            var chipInstance = pool.Get();
            chipInstance.transform.SetParent(parent, false);

            _placedChips.Add(new PlacedChip { ChipId = chipId, Instance = chipInstance });
            return chipInstance.gameObject;
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
                if (_chipPools.TryGetValue(chip.ChipId, out var pool))
                {
                    pool.Return(chip.Instance);
                }
            }

            _placedChips.Clear();
        }
    }
}