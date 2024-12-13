using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public double MatchDurationTime { get; private set; }
    }
}