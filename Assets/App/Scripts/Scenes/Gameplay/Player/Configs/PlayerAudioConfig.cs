using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerAudioConfig", menuName = "Configs/Player/Audio")]
    public class PlayerAudioConfig : SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<string, List<AudioClip>> AudioClips { get;  private set; }
    }
}