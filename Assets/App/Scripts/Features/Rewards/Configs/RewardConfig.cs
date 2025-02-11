using System;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.MinMaxValue;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Rewards.Configs
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/Rewards/Reward")]
    public class RewardConfig : ScriptableObject
    {
        [field: SerializeField, ReadOnly] public string Id { get; private set; }
        [field: SerializeField] public ItemConfig Reward { get; private set; }
        [field: SerializeField] public bool IsFixedCount { get; private set; } = true;
        [field: SerializeField, ShowIf(@"IsFixedCount")] public int Count { get; set; }

        [field: SerializeField, HideIf("IsFixedCount")] public MinMaxInt CountRange { get; set; }

        public void UpdateConfig()
        {
            if (!IsFixedCount)
            {
                Count = CountRange.GetRandom();
            }
        }

        private void OnValidate()
        {
            UpdateConfig();
            Id = name;
        }
    }
}