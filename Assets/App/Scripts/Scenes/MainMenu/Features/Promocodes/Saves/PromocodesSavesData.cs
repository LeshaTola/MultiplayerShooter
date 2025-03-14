using System;
using System.Collections.Generic;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Saves
{
    [Serializable]
    public class PromocodesSavesData
    {
        public List<PromocodeData> UsedPromocodes = new();
    }
}