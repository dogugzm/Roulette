using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "Chip", menuName = "Roulette/Chip")]
    public class ChipSO : UnityEngine.ScriptableObject
    {
        [SerializeField] private int value;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject chipPrefab;
        public GameObject ChipPrefab => chipPrefab;
        public Sprite Sprite => sprite;
        public int Value => value;
    }
}