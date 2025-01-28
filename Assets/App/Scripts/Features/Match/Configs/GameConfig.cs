using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Features.Match.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public double MatchDurationTime { get; private set; } = 600;
        [field: SerializeField] public int RespawnTime { get; private set; } = 3;
        [field: SerializeField] public int EndGameTime { get; private set; } = 10;
    }
}