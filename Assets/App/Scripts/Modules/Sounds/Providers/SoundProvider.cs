using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modules.Sounds.Providers
{
    public class SoundProvider : MonoBehaviour, ISoundProvider
    {
        [SerializeField] private AudioDatabase audioDatabase;
        [SerializeField] private AudioSource audioSource;
        
        public void PlaySound(string key, float volume = 1f)
        {
            if (!audioDatabase.Audios.ContainsKey(key))
            {
                Debug.LogError($"Unknown audio: {key}");
                return;
            }
            PlaySound(audioDatabase.Audios[key]);
        }
        
        public  void PlaySound(List<AudioClip> audioClips, float volume = 1f)
        {
            var audioClip = audioClips[Random.Range(0, audioClips.Count)];
            PlaySound(audioClip, volume);
        }
        
        public void PlaySound(AudioClip audioClip, float volume = 1f)
        {
            audioSource.PlayOneShot(audioClip, volume);
        }

    }
}