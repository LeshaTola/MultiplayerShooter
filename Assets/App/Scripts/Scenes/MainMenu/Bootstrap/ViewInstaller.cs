using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using App.Scripts.Scenes.MainMenu.Features.Roulette;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class ViewInstaller : MonoInstaller
    {
        [Header("Main")]
        [SerializeField] private MainScreen _mainScreen;

        [SerializeField] private UserStatsView _userStatsView;

        [Header("Rooms")]
        [SerializeField] private RoomsView _roomsView;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private InventoryScreeen _inventoryScreeen;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private GameInventoryView _gameInventoryView;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private TabSwitcher _tabSwitcher;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private RectTransform _overlayContainer;

        [Space]
        [SerializeField]
        [FoldoutGroup("Inventory")]
        private InventoryTab _weaponTab;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private WeaponStatsView _weaponStatsView;

        [FoldoutGroup("Inventory")]
        [SerializeField]
        private InventoryTab _equipmentTab;

        [Space]
        [FoldoutGroup("Inventory")]
        [SerializeField]
        private InventoryTab _skinTab;

        [FoldoutGroup("Inventory")]
        [SerializeField]
        private InventorySlot _skinSlot;

        [FoldoutGroup("Inventory")]
        [SerializeField]
        private SkinsView _skinsView;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private ShopScreen _shopScreen;

        [Header("Roulette")]
        [SerializeField] private RouletteConfig _rouletteConfig;

        [SerializeField] private RouletteScreen _rouletteScreen;
        [SerializeField] private RouletteView _rouletteView;

        [Header("TopScreen")]
        [SerializeField] private TopView _topView;

        [SerializeField] private SettingsView _settingsView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainScreenPresenter>().AsSingle();
            Container.BindInstance(_mainScreen).AsSingle();
            Container.BindInstance(_userStatsView).AsSingle();

            Container.BindInterfacesAndSelfTo<RoomsViewElementPresenter>().AsSingle();
            Container.BindInstance(_roomsView).AsSingle();

            BindInventoryScreen();
            BindRouletteScreen();
            BindShopScreen();


            Container.Bind<SettingsView>().FromInstance(_settingsView).AsSingle();
            Container.Bind<TopView>().FromInstance(_topView).AsSingle();
            Container.BindInterfacesAndSelfTo<TopViewPrezentor>().AsSingle();
        }

        private void BindShopScreen()
        {
            Container.BindInstance(_shopScreen).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopScreenElementPrezenter>().AsSingle(); 
        }

        private void BindInventoryScreen()
        {
            Container.BindInterfacesAndSelfTo<InventoryScreenPresenter>().AsSingle();
            Container.Bind<GameInventoryViewPresenter>().AsSingle().WithArguments(_overlayContainer);

            Container.Bind<SelectionProvider>().AsSingle();
            Container.Bind<InventoryTabPresenter>().To<WeaponTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _weaponTab, _weaponStatsView);

            Container.Bind<InventoryTabPresenter>().To<EquipmentTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _equipmentTab);
            Container.Bind<InventoryTabPresenter>().To<SkinsTabPresenter>().AsSingle()
                .WithArguments(_skinsView, _overlayContainer, _skinTab, _skinSlot);
            Container.BindInstance(_inventoryScreeen).AsSingle();
            Container.BindInstance(_gameInventoryView).AsSingle();
            Container.BindInstance(_tabSwitcher).AsSingle();
        }

        private void BindRouletteScreen()
        {
            Container.BindInstance(_rouletteScreen).AsSingle();
            Container.BindInstance(_rouletteView).AsSingle();
            Container.Bind<Roulette>().AsSingle().WithArguments(_rouletteConfig);
            Container.BindInterfacesAndSelfTo<RouletteScreenPresentrer>().AsSingle().WithArguments(_rouletteConfig);
        }
    }
}