using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopupVm
    {
        public PlayerModelsUIProvider PlayerModelsUIProvider { get; }
        public WeaponModelsUIProvider ModelsUIProvider { get; }
        public InfoPopupRouter InfoPopupRouter { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ShopItemData ShopItemData { get; }
        public UserStatsProvider UserStatsProvider { get; }


        public MarketPopupVm(ShopItemData shopItemData,
            ILocalizationSystem localizationSystem,
            InfoPopupRouter infoPopupRouter,
            UserStatsProvider userStatsProvider, 
            WeaponModelsUIProvider weaponModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider)
        {
            PlayerModelsUIProvider = playerModelsUIProvider;
            ModelsUIProvider = weaponModelsUIProvider;
            UserStatsProvider = userStatsProvider;
            InfoPopupRouter = infoPopupRouter;
            LocalizationSystem = localizationSystem;
            ShopItemData = shopItemData;
        }
    }
}