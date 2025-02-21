using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopupRoutrer
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly InventoryProvider _inventoryProvider;
        private readonly Transform _spawnPointTransform;

        public MarketPopupRoutrer(
            IPopupController popupController,
            ILocalizationSystem localizationSystem,
            InfoPopupRouter infoPopupRouter,
            UserStatsProvider userStatsProvider,
            Transform spawnPointTransform)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
            _infoPopupRouter = infoPopupRouter;
            _userStatsProvider = userStatsProvider;
            _spawnPointTransform = spawnPointTransform;
        }

        private MarketPopup _popup;

        public async UniTask ShowPopup(ShopItemData shopItemData)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<MarketPopup>();
            }

            var viewModule = new MarketPopupVm(shopItemData,
                _localizationSystem,
                _infoPopupRouter,
                _spawnPointTransform,
                _userStatsProvider);
            _popup.Setup(viewModule);

            await _popup.Show();
        }

        public async UniTask HidePopup()
        {
            if (_popup == null)
            {
                return;
            }

            await _popup.Hide();
            _popup = null;
        }

        private async void Hide()
        {
            await HidePopup();
        }
    }
}