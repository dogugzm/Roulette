using System;
using Services;
using UnityEngine;

namespace UI
{
    interface IChipManager
    {
        int CurrentChipValue { get; }
        void SetCurrentChipValue(int value);
        Action<Transform> OnChipPlaced { get; set; }
    }

    public class ChipManager : IChipManager
    {
        private IBettingManager _bettingManager;

        public int CurrentChipValue { get; private set; } = -1;

        public ChipManager(IBettingManager bettingManager)
        {
            _bettingManager = bettingManager;
            if (_bettingManager != null)
            {
                _bettingManager.OnBetsCleared += HandleBetsCleared;
            }
        }

        public void SetCurrentChipValue(int value)
        {
            CurrentChipValue = value;
        }

        public Action<Transform> OnChipPlaced { get; set; }

        private void Dispose()
        {
            if (_bettingManager != null)
            {
                _bettingManager.OnBetsCleared -= HandleBetsCleared;
            }
        }

        private void HandleBetsCleared()
        {
        }
    }
}