using System;
using System.Linq;
using App.Scripts.Features.Inventory.Weapons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Features.Inventory
{
    public class InventoryProvider
    {
        public GameInventory GameInventory { get; }
        public GlobalInventory GlobalInventory { get; }

        public InventoryProvider(GameInventory gameInventory, GlobalInventory globalInventory)
        {
            GameInventory = Object.Instantiate(gameInventory);
            GlobalInventory = Object.Instantiate(globalInventory);
        }
        
        public ItemConfig GetConfigById(string id)
        {
            ItemConfig config 
                = GlobalInventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            
            if (config == null)
                config 
                    = GlobalInventory.Equipment.FirstOrDefault(x => x.Id.Equals(id));
            
            if (config == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            return config;
        }
    }
}