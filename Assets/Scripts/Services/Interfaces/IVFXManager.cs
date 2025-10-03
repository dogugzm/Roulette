using UnityEngine;

namespace Services.Interfaces
{
    public interface IVFXManager
    {
        void PlayVFX(string key, Vector3 position, Quaternion rotation, float duration = 2f);
    }
}