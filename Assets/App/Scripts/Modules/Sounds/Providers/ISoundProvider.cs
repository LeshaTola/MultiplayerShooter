using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modules.Sounds.Providers
{
    public interface ISoundProvider
    {
        void PlaySound(string key, float volume = 1f);
        void PlaySound(List<AudioClip> audioClips, float volume = 1f);
        void PlaySound(AudioClip audioClip, float volume = 1f);
    }
}