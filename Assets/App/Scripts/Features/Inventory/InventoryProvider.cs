using System;
using System.Linq;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Features.Inventory
{
    public class InventoryProvider
    {
       // private readonly InventoryConfig _inventoryConfig;

        public GameInventoryData GameInventory { get; private set; } = new();
        public InventoryData Inventory { get; private set; } = new();
        public GlobalInventory GlobalInventory { get; }

        public InventoryProvider(InventoryConfig inventoryConfig, GlobalInventory globalInventory)
        {
            //_inventoryConfig = Object.Instantiate(inventoryConfig);
            GlobalInventory = Object.Instantiate(globalInventory);
            
            GameInventory.Weapons = inventoryConfig.Weapons.Select(x=>x?.Id ?? "").ToList();
            GameInventory.Equipment = inventoryConfig.Equipment.Select(x=>x?.Id ?? "").ToList();
            GameInventory.Skin = inventoryConfig.Skin?.Id ?? "";
            
            Inventory.Weapons = inventoryConfig.PlayerInventory.Weapons.Select(x=>x?.Id ?? "").ToList();
            Inventory.Equipment = inventoryConfig.PlayerInventory.Equipment.Select(x=>x?.Id ?? "").ToList();
            Inventory.Skins = inventoryConfig.PlayerInventory.SkinConfigs.Select(x=>x?.Id ?? "").ToList();
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
    }
}