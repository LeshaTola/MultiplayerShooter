using System;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Saves
{
    [Serializable]
    public class PromocodeData
    {
        public string PromoCode { get; set; }
        public int Uses { get; set; }
    }
}