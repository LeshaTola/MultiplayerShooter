using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopupVm
    {
        public MarketService MarketService { get; }
        public PlayerModelsUIProvider PlayerModelsUIProvider { get; }
        public WeaponModelsUIProvider WeaponsModelsUIProvider { get; }
        public InfoPopupRouter InfoPopupRouter { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ShopItemData ShopItemData { get; }
        public ISoundProvider SoundProvider { get; }


        public MarketPopupVm(ShopItemData shopItemData,
            ILocalizationSystem localizationSystem,
            InfoPopupRouter infoPopupRouter,
            MarketService marketService,
            WeaponModelsUIProvider weaponWeaponsModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider, 
            ISoundProvider soundProvider)
        {
            MarketService = marketService;
            PlayerModelsUIProvider = playerModelsUIProvider;
            SoundProvider = soundProvider;
            WeaponsModelsUIProvider = weaponWeaponsModelsUIProvider;
            InfoPopupRouter = infoPopupRouter;
            LocalizationSystem = localizationSystem;
            ShopItemData = shopItemData;
        }
    }
}