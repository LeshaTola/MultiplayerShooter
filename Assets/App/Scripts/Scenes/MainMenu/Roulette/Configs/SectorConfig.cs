using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Roulette.Configs
{
    [System.Serializable]
    public class SectorConfig
    {
        public string Name;
        [Range(0, 1)] public float Percent;
        public Color Color = Color.white;
        public List<ItemConfig> WinItems;
    }
}