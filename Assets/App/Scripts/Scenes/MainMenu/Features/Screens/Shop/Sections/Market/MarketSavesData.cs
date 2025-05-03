using System;
using System.Collections.Generic;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections
{
    [Serializable]
    public class MarketSavesData
    {
        public List<string> Weapons =new List<string>();
        public List<string> Skins =new List<string>();
        public long LastUpdate;
    }
}