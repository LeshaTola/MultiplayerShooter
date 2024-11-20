using UnityEngine;
using UnityEngine.Audio;

namespace App.Scripts.Modules.Sounds.Services
{
    public class AudioService : IAudioService
    {
        private AudioMixer audioMixer;

        public AudioService(AudioMixer audioMixer)
        {
            this.audioMixer = audioMixer;
        }

        private float masterVolume;
        private float musicVolume;
        private float effectsVolume;

        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = value;
                audioMixer.SetFloat("MasterVolume", ConvertVolume(masterVolume));
            }
        }

        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                musicVolume = value;
                audioMixer.SetFloat("MusicVolume", ConvertVolume(musicVolume));
            }
        }

        public float EffectsVolume
        {
            get => effectsVolume;
            set
            {
                effectsVolume = value;
                audioMixer.SetFloat("EffectsVolume", ConvertVolume(effectsVolume));
            }
        }

        private float ConvertVolume(float value)
        {
            value = Mathf.Clamp01(value);
            return Mathf.Lerp(-80f, 0f, value);
        }
    }
}