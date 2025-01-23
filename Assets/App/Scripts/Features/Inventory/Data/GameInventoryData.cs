using System;
using System.Collections.Generic;

namespace App.Scripts.Features.Inventory
{
    [Serializable]
    public class GameInventoryData
    {
        public string Skin;
        public List<string> Weapons;
        public List<string> Equipment;
    }
}