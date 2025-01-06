using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public double MatchDurationTime { get; private set; }
        [field: SerializeField] public int RespawnTime { get; private set; } = 3;
    }
}