using App.Scripts.Features;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopup : Popup
    {
        [SerializeField] private MarketView _marketView;

        [Header("Buy")]
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _costText;

        [Header("Close")]
        [SerializeField] private TMPLocalizedButton _localizedCloseButton;
        [SerializeField] private Button _closeButton;

        private MarketPopupVm _vm;

        public void Setup(MarketPopupVm vm)
        {
            _vm = vm;
            Initialize();
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            Cleanup();
        }

        private void Initialize()
        {
            _marketView.Initialize(_vm);

            _localizedCloseButton.Initialize(_vm.LocalizationSystem);

            SetupInternal();
        }

        private void SetupInternal()
        {
            Setup3dModel();
            _localizedCloseButton.UpdateAction(Close);
            _closeButton.onClick.AddListener(Close);
            _buyButton.onClick.AddListener(Buy);
            _costText.text = $"{_vm.ShopItemData.Price} {ConstStrings.R}";
        }

        private void Cleanup()
        {
            _localizedCloseButton.Cleanup();
            _closeButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
        }

        private void Setup3dModel()
        {
            switch (_vm.ShopItemData.Item)
            {
                case WeaponConfig weaponConfig:
                    _marketView.SetupWeapon(weaponConfig);
                    break;
                case SkinConfig skinConfig:
                    _marketView.SetupSkin(skinConfig);
                    break;
            }
        }

        private void Close()
        {
            Hide().Forget();
        }

        private void Buy()
        {
            if (!_vm.UserStatsProvider.CoinsProvider.IsEnough(_vm.ShopItemData.Price))
            {
                ShowNotEnoughMoneyMessage();
                return;
            }
            
            ChangeMoney();
            AddItem();
            UpdateInventory();
            
            Close();
        }

        private void ShowNotEnoughMoneyMessage()
        {
            _vm.InfoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
        }

        private void ChangeMoney()
        {
            _vm.UserStatsProvider.CoinsProvider.ChangeCoins(-_vm.ShopItemData.Price);
        }

        private void UpdateInventory()
        {
            _vm.UserStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
            _vm.UserStatsProvider.SaveState();
        }

        private void AddItem()
        {
            switch (_vm.ShopItemData.Item)
            {
                case WeaponConfig weaponConfig:
                    _vm.UserStatsProvider.InventoryProvider.Inventory.Weapons.Add(weaponConfig.Id);
                    break;
                case SkinConfig skinConfig:
                    _vm.UserStatsProvider.InventoryProvider.Inventory.Skins.Add(skinConfig.Id);
                    break;
            }
        }
    }
}