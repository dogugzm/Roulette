using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    interface IChipManager
    {
        GameObject TryPlaceChip(Transform parent);
        ChipSO CurrentChipSo { get; set; }
        void RestoreState(IReadOnlyList<Bet> bets);
    }
}