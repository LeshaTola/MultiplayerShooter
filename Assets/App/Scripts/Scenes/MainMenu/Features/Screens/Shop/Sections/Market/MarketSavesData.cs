using System;
using System.Collections.Generic;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections
{
    [Serializable]
    public class MarketSavesData
    {
        public List<string> CurrentItems;
        public DateTime LastUpdate;
    }
}