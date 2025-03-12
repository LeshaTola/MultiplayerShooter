using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Configs
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Rarity", fileName = "RaritiesDatabase")]
    public class RaritiesDatabase : SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<string, Rarity> Rarities { get; private set; }
        [field: SerializeField] public List<string> RarityOrder { get; private set; }
        
        public List<ItemConfig> SortByRarity(List<ItemConfig> items)
        {
            var rarityOrderDict = RarityOrder
                .Select((rarity, index) => new { rarity, index })
                .ToDictionary(x => x.rarity, x => x.index);

            var sortedItems = items
                .OrderBy(id => 
                    Rarities.TryGetValue(id.Rarity, out var rarity) 
                        ? rarityOrderDict.GetValueOrDefault(rarity.Name, int.MaxValue) 
                        : int.MaxValue
                )
                .ToList();

            return sortedItems;
        }
        
    }
}