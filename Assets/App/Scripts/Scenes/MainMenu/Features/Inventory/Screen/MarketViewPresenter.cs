using System;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Commands;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Screen
{
    public class MarketViewPresenter
    {
        private readonly ILocalizationSystem _localizationSystem;
        private readonly WeaponModelsUIProvider _weaponModelsUIProvider;
        private readonly PlayerModelsUIProvider _playerModelsUIProvider;

        private readonly MarketView _marketViewView;
        private readonly BuyItemCommand _buyItemCommand;
        private readonly EquipItemCommand _equipItemCommand;
        private readonly InventoryProvider _inventoryProvider;
        private readonly MarketService _marketService;
        private readonly SelectionProvider _selectionProvider;
        private Action _marketViewAction;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly ISoundProvider _soundProvider;

        public MarketViewPresenter(ILocalizationSystem localizationSystem,
            WeaponModelsUIProvider weaponModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider,
            InventoryProvider inventoryProvider,
            MarketService marketService,
            MarketView marketViewView,
            SelectionProvider selectionProvider,
            EquipItemCommand equipItemCommand, 
            InfoPopupRouter infoPopupRouter, ISoundProvider soundProvider)
        {
            _localizationSystem = localizationSystem;
            _weaponModelsUIProvider = weaponModelsUIProvider;
            _playerModelsUIProvider = playerModelsUIProvider;
            _inventoryProvider = inventoryProvider;
            _marketService = marketService;
            _marketViewView = marketViewView;
            _selectionProvider = selectionProvider;
            _equipItemCommand = equipItemCommand;
            _infoPopupRouter = infoPopupRouter;
            _soundProvider = soundProvider;
        }

        public void Initialize()
        {
            _marketViewView.Initialize(_localizationSystem, _weaponModelsUIProvider, _playerModelsUIProvider);

            _marketViewView.OnButtonClicked += OnButtonClicked;

            _selectionProvider.OnWeaponSelected += OnWeaponSelected;
            _selectionProvider.OnSkinSelected += OnSkinSelected;
        }

        public void Cleanup()
        {
            _marketViewView.Cleanup();

            _selectionProvider.OnWeaponSelected -= OnWeaponSelected;
            _selectionProvider.OnSkinSelected -= OnSkinSelected;
        }

        public void OnSkinSelected(string id)
        {
            var config = _inventoryProvider.GlobalInventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(id));
            SelectSkin(config);
        }

        private void SelectSkin(SkinConfig config)
        {
            if (config)
            {
                _marketViewView.SetupSkin(config);
                var isInInventory = _inventoryProvider.Inventory.Skins.Contains(config.Id);

                _marketViewView.SetButtonActive(true);
                if (!isInInventory)
                {
                    SetupBuyAction(config);
                }
                else
                {

                    var isSameItem = !_inventoryProvider.GameInventory.Skin.Equals(config.Id);
                    _marketViewView.SetButtonActive(isSameItem);
                    
                    _marketViewView.SetupButtonText(ConstStrings.EQUIP);
                    _marketViewAction = () =>
                    {
                        _equipItemCommand.Skin = config;
                        _equipItemCommand.Execute();
                        _marketViewView.SetButtonActive(false);
                    };
                }
            }
        }

        public void OnWeaponSelected(string id)
        {
            var config = _inventoryProvider.GlobalInventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            OnWeaponSelected(config);
        }

        public void OnWeaponSelected(WeaponConfig config)
        {
            if (config)
            {
                _marketViewView.SetupWeapon(config);
                var isInInventory = _inventoryProvider.Inventory.Weapons.Contains(config.Id);
                _marketViewView.SetButtonActive(!isInInventory);
                if (!isInInventory)
                {
                    SetupBuyAction(config);
                }
            }
        }

        private void SetupBuyAction(ItemConfig config)
        {
            var shopItemData = _marketService.GetShopItemDataByMarket(config);
            _marketViewView.SetupButtonText(shopItemData.Price.ToString() + ConstStrings.R);

            SetupAction(shopItemData);
        }

        private void SetupAction(ShopItemData shopItemData)
        {
            _marketViewAction = () =>
            {
                if (!_marketService.TryBuyItem(shopItemData))
                {
                    ShowNotEnoughMoneyMessage();
                    return;
                }
            
                _soundProvider.PlaySound("Buy_sound");
                _marketViewView.SetButtonActive(false);
            };
        }
        
        private void ShowNotEnoughMoneyMessage()
        {
            _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
        }

        private void OnButtonClicked()
        {
            _marketViewAction?.Invoke();
        }
    }
}