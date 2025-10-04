using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Roulette/Sound Database")]
    public class SoundDatabaseSO : UnityEngine.ScriptableObject
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
            public AudioChannel channel;
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