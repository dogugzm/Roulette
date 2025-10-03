using System.Collections.Generic;
using Models;
using ScriptableObject;
using UnityEngine;

namespace Services.Interfaces
{
    interface IChipManager
    {
        GameObject TryPlaceChip(Transform parent);
        ChipSO CurrentChipSo { get; set; }
        void RestoreState(ChipSO currentChipSo);
    }
}