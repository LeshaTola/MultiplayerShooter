using System;
using System.Collections.Generic;

namespace App.Scripts.Features.Inventory
{
    [Serializable]
    public class GameInventoryData
    {
        public event Action OnInventoryUpdated;

        public string Skin;
        public List<string> Weapons;
        public List<string> Equipment;
    
        public void InvokeInventoryUpdate()
        {
            OnInventoryUpdated?.Invoke();
        }
    }
}