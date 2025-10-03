using System.Collections.Generic;
using Models;
using ScriptableObject;
using UnityEngine;

namespace Services.Interfaces
{
    internal interface IChipManager
    {
        GameObject TryPlaceChip(Transform parent);
        ChipSO CurrentChipSo { get; set; }
        void RestoreState(string currentChipId);
        GameObject PlaceChipById(string chipId, Transform parent);
    }
}