using App.Scripts.Features.Match.Maps;
using UnityEngine;

namespace App.Scripts.Features.Match.Configs
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/Maps/Map")]
    public class MapConfig:ScriptableObject
    {
        [field:SerializeField] public string Name { get; private set;}
        [field:SerializeField] public Sprite Sprite { get; private set;}
        [field:SerializeField] public Map Prefab { get; private set; }
    }
}