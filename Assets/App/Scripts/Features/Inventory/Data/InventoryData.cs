using System;
using System.Collections.Generic;

namespace App.Scripts.Features.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public List<string> Skins;
        public List<string> Weapons;
        public List<string> Equipment;
    }
}