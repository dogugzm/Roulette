using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "VFXDatabase", menuName = "SO/VFXDatabase")]
    public class VFXDatabaseSO : UnityEngine.ScriptableObject
    {
        public List<VFXData> vfxList;

        public GameObject GetVFX(string key)
        {
            return vfxList.Find(vfx => vfx.key == key)?.vfxPrefab;
        }
    }

    [Serializable]
    public class VFXData
    {
        public string key;
        public GameObject vfxPrefab;
    }
}