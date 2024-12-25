using App.Scripts.Modules.MinMaxValue;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
    public class PlayerConfig : ScriptableObject
    {
        [field: Header("Health")]
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;

        [field: Header("Movement")]
        [field: SerializeField] public float Speed { get; private set; } = 2.0f;
        [field: SerializeField] public float JumpHeight { get; private set; } = 1.0f;
        [field: SerializeField] public float JumpFallSpeed { get; private set; } = 1.0f;
        [field: SerializeField] public float ImmortalTime { get; private set; } = 3.0f;
    }
}