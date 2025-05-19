using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly WeaponModelsUIProvider _weaponModelsUIProvider;
        private readonly PlayerModelsUIProvider _playerModelsUIProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly InventoryProvider _inventoryProvider;
        private readonly ISoundProvider _soundProvider;
        private readonly MarketService _marketService;

        public MarketPopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem,
            InfoPopupRouter infoPopupRouter,
            WeaponModelsUIProvider weaponModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider,
            ISoundProvider soundProvider,
            MarketService marketService)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
            _infoPopupRouter = infoPopupRouter;
            _weaponModelsUIProvider = weaponModelsUIProvider;
            _playerModelsUIProvider = playerModelsUIProvider;
            _soundProvider = soundProvider;
            _marketService = marketService;
        }

        public async UniTask ShowPopup(ShopItemData shopItemData)
        {
            var popup = _popupController.GetPopup<MarketPopup>();

            var viewModule = GetVm(shopItemData);
            popup.Setup(viewModule);

            await popup.Show();
        }

        private MarketPopupVm GetVm(ShopItemData shopItemData)
        {
            var viewModule = new MarketPopupVm(shopItemData,
                _localizationSystem,
                _infoPopupRouter,
                _marketService,
                _weaponModelsUIProvider,
                _playerModelsUIProvider, _soundProvider);
            return viewModule;
        }
    }
}