using App.Scripts.Features;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization.Localizers;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class SkinPopup : MarketPopup
    {
        [SerializeField] private TMPLocalizer _header;
        [SerializeField] private TMPLocalizer _rarity;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _buyButton;

        [SerializeField] private Transform _spawnPoint;
        

        public override void Setup(MarketPopupVm vm)
        {
            base.Setup(vm);

            Initialize();
            LocalSetup();
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            Cleanup();
            Vm.PlayerModelsUIProvider.Cleanup();
        }

        private void Cleanup()
        {
            _header.Cleanup();
            _rarity.Cleanup();
            
            _closeButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
        }

        private void Initialize()
        {
            _header.Initialize(Vm.LocalizationSystem);
            _rarity.Initialize(Vm.LocalizationSystem);
        }

        private void LocalSetup()
        {
            _closeButton.onClick.AddListener(Close);
            _buyButton.onClick.AddListener(Buy);

            _costText.text = Vm.ShopItemData.Price + ConstStrings.R;
            var config = Vm.ShopItemData.Item;

            _header.Key = config.name;
            _rarity.Key = config.Rarity;
            _header.Translate();
            _rarity.Translate();
            
            Vm.PlayerModelsUIProvider.Setup(config.Id);
        }

        private void Close()
        {
            Hide().Forget();
        }

        private void Buy()
        {
            if (!Vm.UserStatsProvider.CoinsProvider.IsEnough(Vm.ShopItemData.Price))
            {
                Vm.InfoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
                return;
            }
            
            Vm.UserStatsProvider.CoinsProvider.ChangeCoins(-Vm.ShopItemData.Price);
            if (Vm.ShopItemData.Item is SkinConfig skinConfig)
            {
                Vm.UserStatsProvider.InventoryProvider.Inventory.Skins.Add(skinConfig.Id);
                Vm.UserStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
                Vm.UserStatsProvider.SaveState();
            }

            Close();
        }
    }
}