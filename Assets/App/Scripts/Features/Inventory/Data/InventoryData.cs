using System;
using System.Collections.Generic;

namespace App.Scripts.Features.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public HashSet<string> Skins;
        public HashSet<string> Weapons;
        public HashSet<string> Equipment;
    }
}