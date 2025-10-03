using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAudioManager
    {
        void PlaySound(string key, float volume = 1f);
        void StopSound(string key);
        void StopAllSounds();
        void SetVolume(string key, float volume);
        Task StopSoundFadedAsync(string key, float fadeDuration);
    }
}