using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Scripts.Features.Inventory.Data
{
    [Serializable]
    public class InventoryData
    {
        public event Action OnInventoryUpdated;
        
        public HashSet<string> Skins = new();
        public HashSet<string> Weapons = new();
        public HashSet<string> Equipment = new();

        public void InvokeInventoryUpdate()
        {
            OnInventoryUpdated?.Invoke();
        }

        public InventoryDataForSaves ToSavesData()
        {
            return new()
            {
                Skins = Skins.ToList(),
                Weapons = Weapons.ToList(),
                Equipment = Equipment.ToList(),
            };
        }
    }
    
    public class InventoryDataForSaves
    {
        public List<string> Skins = new();
        public List<string> Weapons = new();
        public List<string> Equipment = new();
    }
}