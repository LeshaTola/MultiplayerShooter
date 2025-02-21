using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopupVm
    {
        public Transform SpawnPoint { get; }
        public InfoPopupRouter InfoPopupRouter { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ShopItemData ShopItemData { get; }
        public UserStatsProvider UserStatsProvider { get; }


        public MarketPopupVm(ShopItemData shopItemData,
            ILocalizationSystem localizationSystem,
            InfoPopupRouter infoPopupRouter,
            Transform spawnPoint, UserStatsProvider userStatsProvider)
        {
            SpawnPoint = spawnPoint;
            UserStatsProvider = userStatsProvider;
            InfoPopupRouter = infoPopupRouter;
            LocalizationSystem = localizationSystem;
            ShopItemData = shopItemData;
        }
    }
}