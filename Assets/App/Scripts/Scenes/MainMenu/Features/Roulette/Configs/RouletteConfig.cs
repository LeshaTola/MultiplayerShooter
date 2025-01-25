using System.Collections.Generic;
using App.Scripts.Modules.MinMaxValue;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Configs
{
    [CreateAssetMenu(fileName = "RouletteConfig",menuName = "Configs/Roulette")]
    public class RouletteConfig : ScriptableObject
    {
        [field: SerializeField] public float SpinDuration { get; private set; } = 3f;
        [field: SerializeField] public float ShowDuration { get; private set; } = 0.5f;
        [field: SerializeField] public MinMaxInt SpinCount { get; private set; }
        [field: SerializeField] public List<SectorConfig> Sectors { get; private set; }
        [field: SerializeField] public AnimationCurve SpinCurve { get; private set; }
    }
}