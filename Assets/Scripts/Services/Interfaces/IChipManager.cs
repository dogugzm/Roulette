using System.Threading.Tasks;
using ScriptableObject;
using UnityEngine;

namespace Services.Interfaces
{
    internal interface IChipManager
    {
        Task InitializeAsync();
        GameObject TryPlaceChip(Transform parent);
        ChipSO CurrentChipSo { get; set; }
        void RestoreState(string currentChipId);
        GameObject PlaceChipById(string chipId, Transform parent);
    }
}