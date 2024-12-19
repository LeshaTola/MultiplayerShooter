using System;
using System.Linq;
using App.Scripts.Features.Inventory.Weapons;

namespace App.Scripts.Features.Inventory
{
    public class InventoryProvider
    {
        public GameInventory GameInventory { get; }
        public GlobalInventory GlobalInventory { get; }

        public InventoryProvider(GameInventory gameInventory, GlobalInventory globalInventory)
        {
            GameInventory = gameInventory;
            GlobalInventory = globalInventory;
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