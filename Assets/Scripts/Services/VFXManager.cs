
using System.Threading.Tasks;
using UnityEngine;

public static class VFXConstants
{
    public const string ChipPlaced = "chip_placed";
    public const string WinEffect = "win_effect";
    public const string LoseEffect = "lose_effect";
}

public class VFXManager : IVFXManager
{
    private readonly VFXDatabaseSO _vfxDatabase;

    public VFXManager(VFXDatabaseSO vfxDatabase)
    {
        _vfxDatabase = vfxDatabase;
    }

    public void PlayVFX(string key, Vector3 position, Quaternion rotation, float duration = 2f)
    {
        var vfxPrefab = _vfxDatabase.GetVFX(key);
        if (vfxPrefab == null) return;

        var vfx = Object.Instantiate(vfxPrefab, position, rotation);
        Object.Destroy(vfx, duration);
    }
}
