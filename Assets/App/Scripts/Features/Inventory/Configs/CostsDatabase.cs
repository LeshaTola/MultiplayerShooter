using System.Collections.Generic;
using App.Scripts.Features.Rewards.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Configs
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Cost", fileName = "CostsDatabase")]
    public class CostsDatabase : SerializedScriptableObject
    {
        [SerializeField] private RewardConfig _coinReward;
        [SerializeField] private List<(string, int)> _costByRarity;
        [SerializeField] private RaritiesDatabase _raritiesDatabase;
        
        [Button]
        public void ConnectRarity()
        {
            PriceByRarity = new();
            foreach (var item in _costByRarity)
            {
                if (!_raritiesDatabase.Rarities.TryGetValue(item.Item1, out var rarity))
                {
                    Debug.LogWarning($"Редкость '{item.Item1}' не найдена в базе.");
                    continue;
                }

                if (!PriceByRarity.ContainsKey(rarity.Name))
                {
                    PriceByRarity[rarity.Name] = item.Item2;
                }
            }
        }

        [field: SerializeField, ReadOnly] public Dictionary<string, int> PriceByRarity { get; private set; }

        public RewardConfig GetCoinRewardByRarity(string rarity)
        {
            var coinReward = Instantiate(_coinReward);
            coinReward.Count = PriceByRarity[rarity];
            return coinReward;
        }
    }
}