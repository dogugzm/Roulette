using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "ChipDatabase", menuName = "Roulette/ChipDatabase")]
    public class ChipDatabaseSO : UnityEngine.ScriptableObject
    {
        [SerializeField] private List<ChipSO> chips;

        public ChipSO GetChipByID(string chipId)
        {
            return chips.FirstOrDefault(chip => chip.Id == chipId);
        }

        public List<ChipSO> GetAllChips()
        {
            return chips;
        }
    }
}
