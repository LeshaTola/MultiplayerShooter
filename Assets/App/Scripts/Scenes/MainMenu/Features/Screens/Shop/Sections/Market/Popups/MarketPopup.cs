using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using App.Scripts.Modules.Sounds;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopup : Popup
    {
        [ValueDropdown(@"GetAudioKeys")] [SerializeField] private string _buySound;
        [ValueDropdown(@"GetAudioKeys")] [SerializeField] private string _closeSound;
        
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
            _vm.SoundProvider.PlaySound(_closeSound);
            Hide().Forget();
        }

        private void Buy()
        {
            if (!_vm.MarketService.TryBuyItem(_vm.ShopItemData))
            {
                ShowNotEnoughMoneyMessage();
                return;
            }
            
            _vm.SoundProvider.PlaySound(_buySound);
            Close();
        }

        private void ShowNotEnoughMoneyMessage()
        {
            _vm.InfoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
        }
    }
}