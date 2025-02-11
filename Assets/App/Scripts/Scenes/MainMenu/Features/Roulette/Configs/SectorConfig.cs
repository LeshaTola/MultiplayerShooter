using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Rewards.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Configs
{
    [System.Serializable]
    public class SectorConfig
    {
        public string Name;
        [Range(0, 1)] public float Percent;
        public Color Color = Color.white;
        public List<RewardConfig> WinItems;
        [FormerlySerializedAs("WinItemCount")] public int WinItemsCount;
    }
}