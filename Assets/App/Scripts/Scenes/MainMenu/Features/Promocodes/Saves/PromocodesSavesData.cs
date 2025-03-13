using System;
using System.Collections.Generic;

namespace App.Scripts.Scenes.MainMenu.Features.Promocodes.Saves
{
    [Serializable]
    public class PromocodesSavesData
    {
        public List<PromocodeData> UsedPromocodes = new();
    }
}