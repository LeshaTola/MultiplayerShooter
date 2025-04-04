using System.Runtime.InteropServices;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Strategies
{
    public class DeleteAllSavesPromocode : PromocodeAction
    {
        public override void Execute()
        {
            YG2.saves.PlayerName = string.Empty;
            YG2.saves.UserStatsData = null;
            YG2.saves.SettingsData = null;
            YG2.saves.TasksData = null;
            YG2.saves.PromocodesesData = null;
            YG2.saves.MarketData = null;
            YG2.SaveProgress();

            Reload();
        }
        
        [DllImport("__Internal")]
        private static extern void ReloadPage();

        public void Reload()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ReloadPage();
#else
            Debug.LogWarning("Reload game");
#endif
        }
    }
}