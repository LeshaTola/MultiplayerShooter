using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory
{
    [CreateAssetMenu(menuName = "Configs/Inventory/GlobalInventory", fileName = "GlobalInventory")]
    public class GlobalInventory: SerializedScriptableObject
    {
        [field: SerializeField] public List<SkinConfig> SkinConfigs { get; private set; }
        [field: SerializeField] public List<WeaponConfig> Weapons { get; private set; }
        [field: SerializeField] public List<EquipmentConfig> Equipment { get; private set; }

        [SerializeField] private RaritiesDatabase _raritiesDatabase;

        [Button]
        public void ConnectGlobalInventory()
        {
            ItemsByRarity = new Dictionary<string, List<ItemConfig>>();

            // Получаем все предметы
            var allItems = new List<ItemConfig>();
            allItems.AddRange(SkinConfigs);
            allItems.AddRange(Weapons);
            allItems.AddRange(Equipment);

            foreach (var item in allItems)
            {
                if (!_raritiesDatabase.Rarities.TryGetValue(item.Rarity, out var rarity))
                {
                    Debug.LogWarning($"Редкость '{item.Rarity}' не найдена в базе.");
                    continue;
                }

                if (!ItemsByRarity.ContainsKey(rarity.Name))
                {
                    ItemsByRarity[rarity.Name] = new List<ItemConfig>();
                }

                ItemsByRarity[rarity.Name].Add(item);
            }
        }
        
        [field: SerializeField, ReadOnly] public Dictionary<string, List<ItemConfig>> ItemsByRarity { get; private set; }
        
        

    }
}