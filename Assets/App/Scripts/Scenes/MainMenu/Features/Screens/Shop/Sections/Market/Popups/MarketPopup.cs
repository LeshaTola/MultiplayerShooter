using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization.Localizers;
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
        [SerializeField] private TMPLocalizer _header;

        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private WeaponStat _weaponStatPrefab;

        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _buyButton;

        [SerializeField] private Transform _spawnPoint;
        
        private readonly List<WeaponStat> _weaponStats = new();

        private MarketPopupVm _vm;

        public void Setup(MarketPopupVm vm)
        {
            _vm = vm;

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
            _header.Initialize(_vm.LocalizationSystem);
            _vm.SpawnPoint.SetParent(_spawnPoint);
        }

        private void LocalSetup()
        {
            Default();

            _closeButton.onClick.AddListener(Close);
            _buyButton.onClick.AddListener(Buy);

            _costText.text = _vm.ShopItemData.Price + ConstStrings.R;
            var config = _vm.ShopItemData.Item;

            _header.Key = config.name;
            _header.Translate();
            if (config is WeaponConfig weaponConfig)
            {
                foreach (var stat in weaponConfig.Stats)
                {
                    var newStat = Instantiate(_weaponStatPrefab, _statsContainer);
                    newStat.Initialize(_vm.LocalizationSystem);
                    newStat.Setup(stat.Item1, stat.Item2);
                    _weaponStats.Add(newStat);
                }

                var weapon = Instantiate(weaponConfig.Prefab, _vm.SpawnPoint);
                weapon.transform.localPosition += weaponConfig.ViewOffset;
                weapon.transform.localRotation *= Quaternion.Euler(weaponConfig.ViewRotationOffset);
                weapon.transform.localScale = Vector3.Scale(weapon.transform.localScale, weaponConfig.ViewScaleMultiplier);
            }
        }

        private void Default()
        {
            foreach (var weaponStat in _weaponStats)
            {
                weaponStat.Cleanup();
                Destroy(weaponStat.gameObject);
            }

            _weaponStats.Clear();

            foreach (Transform child in _vm.SpawnPoint)
            {
                Destroy(child.gameObject);
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
                _vm.InfoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
                return;
            }
            
            _vm.UserStatsProvider.CoinsProvider.ChangeCoins(-_vm.ShopItemData.Price);
            if (_vm.ShopItemData.Item is WeaponConfig weaponConfig)
            {
                _vm.UserStatsProvider.InventoryProvider.Inventory.Weapons.Add(weaponConfig.Id);
                _vm.UserStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
                _vm.UserStatsProvider.SaveState();
            }

            Close();
        }
    }
}