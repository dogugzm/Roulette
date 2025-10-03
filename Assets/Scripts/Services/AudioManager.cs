using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObject;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public static class SFXConstants
    {
        public const string BackgroundMusic = "background_music";
        public const string Click = "click";
        public const string ChipPut = "put";
        public const string BallSpin = "ball_spin";
        public const string Success = "success";
        public const string BallDrop = "ball_drop";
        public const string Lose = "lose";
    }

    public class AudioManager : IAudioManager
    {
        private readonly Dictionary<SoundDatabaseSO.AudioChannel, AudioSource> _sources;
        private readonly SoundDatabaseSO _soundDatabase;

        public AudioManager(SoundDatabaseSO soundDatabase, AudioSource musicSource, AudioSource fxSource)
        {
            _soundDatabase = soundDatabase;

            _sources = new Dictionary<SoundDatabaseSO.AudioChannel, AudioSource>
            {
                { SoundDatabaseSO.AudioChannel.Music, musicSource },
                { SoundDatabaseSO.AudioChannel.Fx, fxSource }
            };
        }

        public void PlaySound(string key, float volume = 1f)
        {
            var sound = _soundDatabase.GetSound(key);
            if (sound == null) return;

            if (!_sources.TryGetValue(sound.channel, out var source)) return;

            source.clip = sound.clip;
            source.loop = sound.loop;
            source.volume = volume;
            source.Play();
        }

        public void StopSound(string key)
        {
            var sound = _soundDatabase.GetSound(key);
            if (sound == null) return;

            if (_sources.TryGetValue(sound.channel, out var source))
                source.Stop();
        }

        public async Task StopSoundFadedAsync(string key, float fadeDuration = 1f)
        {
            var sound = _soundDatabase.GetSound(key);
            if (sound == null) return;

            if (_sources.TryGetValue(sound.channel, out var source))
            {
                float startVolume = source.volume;
                float time = 0f;

                while (time < fadeDuration)
                {
                    await Task.Yield();
                    time += Time.deltaTime;
                    source.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration);
                }

                source.Stop();
                source.volume = startVolume; // reset for next play
            }
        }


        private IEnumerator FadeOut(AudioSource source, float duration)
        {
            float startVolume = source.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                yield return null;
            }

            source.Stop();
            source.volume = startVolume; // reset volume for next play
        }

        public void StopAllSounds()
        {
            foreach (var source in _sources.Values)
                source.Stop();
        }

        public void SetVolume(string key, float volume)
        {
            var sound = _soundDatabase.GetSound(key);
            if (sound == null) return;

            if (_sources.TryGetValue(sound.channel, out var source))
                source.volume = volume;
        }
    }
}