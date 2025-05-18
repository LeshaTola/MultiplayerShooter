using App.Scripts.Features.Match.Configs;
using App.Scripts.Modules.MinMaxValue;
using UnityEngine;

namespace App.Scripts.Features.GameMods.Configs
{
    [CreateAssetMenu(fileName = "GameModConfig", menuName = "Configs/GameMod")]
    public class GameModConfig : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public MapsConfig Maps { get; private set; }
        [field: SerializeField] public MinMaxInt Players { get; private set; }
        [field: SerializeField] public GameConfig GameConfig { get; private set; }
        [field: SerializeField] public string SceneName { get; private set; }
    }
}