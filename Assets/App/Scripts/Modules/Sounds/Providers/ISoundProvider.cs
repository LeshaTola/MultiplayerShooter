using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modules.Sounds.Providers
{
    public interface ISoundProvider
    {
        void PlayOneShotSound(string key, float volume = 1f);
        void PlayOneShotSound(List<AudioClip> audioClips, float volume = 1f);
        void PlayOneShotSound(AudioClip audioClip, float volume = 1f);
    }
}