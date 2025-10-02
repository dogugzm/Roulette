using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Roulette/Sound Database")]
    public class SoundDatabaseSO : ScriptableObject
    {
        public enum AudioChannel
        {
            Music,
            Fx
        }

        [System.Serializable]
        public class SoundEntry
        {
            public string key;
            public AudioClip clip;
            public bool loop;
            public AudioChannel channel; // hangi source kullanılacak
        }

        public SoundEntry[] sounds;

        public SoundEntry GetSound(string key)
        {
            foreach (var s in sounds)
            {
                if (s.key == key) return s;
            }

            return null;
        }
    }
}