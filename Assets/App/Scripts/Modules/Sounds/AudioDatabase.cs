using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.Sounds
{
    
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Databases/Audio")]
    public class AudioDatabase : SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<string, List<AudioClip>> Audios { get; private set; }

        public List<string> GetKeys()
        {
            return new (Audios.Keys);
        }
    }
}