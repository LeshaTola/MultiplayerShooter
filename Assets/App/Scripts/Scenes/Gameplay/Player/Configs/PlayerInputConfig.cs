using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
    public class PlayerInputConfig:ScriptableObject
    {
        [field: Header("Movement")] 
        [field: SerializeField] public float Speed { get; private set; } = 2.0f;
        [field: SerializeField] public float JumpHeight { get;  private set;} = 1.0f;
        [field: SerializeField] public float JumpFallSpeed { get;  private set;} = 1.0f;
    }
}