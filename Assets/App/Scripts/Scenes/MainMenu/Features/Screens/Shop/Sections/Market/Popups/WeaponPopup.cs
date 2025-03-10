using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class WeaponPopup : MarketPopup
    {
        [SerializeField] private TMPLocalizer _header;

        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private WeaponStat _weaponStatPrefab;

        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _buyButton;

        [SerializeField] private Transform _spawnPoint;
        
        private readonly List<WeaponStat> _weaponStats = new();

        public override void Setup(MarketPopupVm vm)
        {
            base.Setup(vm);

            Initialize();
            LocalSetup();
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            Default();
            Cleanup();
        }

        private void Cleanup()
        {
            _header.Cleanup();
            _closeButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
        }

        private void Initialize()
        {
            _header.Initialize(Vm.LocalizationSystem);
        }

        private void LocalSetup()
        {
            Default();

            _closeButton.onClick.AddListener(Close);
            _buyButton.onClick.AddListener(Buy);

            _costText.text = Vm.ShopItemData.Price + ConstStrings.R;
            var config = Vm.ShopItemData.Item;

            _header.Key = config.name;
            _header.Translate();
            
            if (config is not WeaponConfig weaponConfig)
            {
                return;
            }
            
            SetupStats(weaponConfig);
            Vm.ModelsUIProvider.SetupWeapon(weaponConfig);
        }

        private void SetupStats(WeaponConfig weaponConfig)
        {
            foreach (var stat in weaponConfig.Stats)
            {
                var newStat = Instantiate(_weaponStatPrefab, _statsContainer);
                newStat.Initialize(Vm.LocalizationSystem);
                newStat.Setup(stat.Item1, stat.Item2);
                _weaponStats.Add(newStat);
            }
        }

        private void Default()
        {
            Vm.ModelsUIProvider.Cleanup();
            
            foreach (var weaponStat in _weaponStats)
            {
                weaponStat.Cleanup();
                Destroy(weaponStat.gameObject);
            }

            _weaponStats.Clear();
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
            if (Vm.ShopItemData.Item is WeaponConfig weaponConfig)
            {
                Vm.UserStatsProvider.InventoryProvider.Inventory.Weapons.Add(weaponConfig.Id);
                Vm.UserStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
                Vm.UserStatsProvider.SaveState();
            }

            Close();
        }
    }
}