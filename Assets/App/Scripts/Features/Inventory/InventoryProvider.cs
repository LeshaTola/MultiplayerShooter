using System;
using System.Linq;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Features.Inventory
{
    public class InventoryProvider
    {
        public GameInventoryData GameInventory { get; private set; } = new();
        public InventoryData Inventory { get; private set; } = new();
        public GlobalInventory GlobalInventory { get; }

        public InventoryProvider( GlobalInventory globalInventory)
        {
            GlobalInventory = Object.Instantiate(globalInventory);
        }
        
        public ItemConfig GetConfigById(string id)
        {
            ItemConfig config = WeaponById(id);
            
            if (config == null)
                config = SkinById(id);

            if (config == null)
                config = EquipmentById(id);
            
            if (config == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            return config;
        }

        public EquipmentConfig EquipmentById(string id)
        {
            return GlobalInventory.Equipment.FirstOrDefault(x => x.Id.Equals(id));
        }
        
        public WeaponConfig WeaponById(string id)
        {
            return GlobalInventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
        }

        public SkinConfig SkinById(string id)
        {
            return GlobalInventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SetState(UserStatsData userStats)
        {
            GameInventory  = userStats.GameInventory;
            Inventory = userStats.Inventory; 
        }
    }
}