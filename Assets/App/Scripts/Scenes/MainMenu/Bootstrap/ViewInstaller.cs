using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Inventory.Tabs.Weapons;
using App.Scripts.Scenes.MainMenu.Roulette;
using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using App.Scripts.Scenes.MainMenu.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using App.Scripts.Scenes.MainMenu.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using App.Scripts.Scenes.MainMenu.UserProfile;
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

        [Header("Inventory")]
        [SerializeField] private InventoryScreeen _inventoryScreeen;

        [SerializeField] private GameInventoryView _gameInventoryView;

        [SerializeField] private TabSwitcher _tabSwitcher;
        [SerializeField] private RectTransform _overlayContainer;

        [Space]
        [SerializeField] private InventoryTab _weaponTab;
        [SerializeField] private WeaponStatsView _weaponStatsView;

        [SerializeField] private InventoryTab _equipmentTab;

        [Space]
        [SerializeField] private InventoryTab _skinTab;

        [SerializeField] private InventorySlot _skinSlot;

        [Header("Roulette")]
        [SerializeField] private RouletteConfig _rouletteConfig;

        [SerializeField] private RouletteScreen _rouletteScreen;
        [SerializeField] private RouletteView _rouletteView;

        [Header("TopScreen")]
        [SerializeField] private TopView _topView;

        [SerializeField] private SettingsView _settingsView;

        [SerializeField] [SerializeReference] [OdinSerialize]
        private List<Type> _states = new()
        {
            typeof(RoomState),
            typeof(InventoryState),
            typeof(InventoryState),
            typeof(MainScreen),
        };

        public override void InstallBindings()
        {
            Container.Bind<MainScreenPresenter>().AsSingle();
            Container.BindInstance(_mainScreen).AsSingle();
            Container.BindInstance(_userStatsView).AsSingle();

            Container.BindInterfacesAndSelfTo<RoomsViewPresenter>().AsSingle();
            Container.BindInstance(_roomsView).AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryScreenPresenter>().AsSingle();
            Container.Bind<GameInventoryViewPresenter>().AsSingle().WithArguments(_overlayContainer);
            
            Container.Bind<SelectionProvider>().AsSingle();
            Container.Bind<InventoryTabPresenter>().To<WeaponTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _weaponTab, _weaponStatsView);
            
            Container.Bind<InventoryTabPresenter>().To<EquipmentTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _equipmentTab);
            Container.Bind<InventoryTabPresenter>().To<SkinsTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _skinTab, _skinSlot);
            Container.BindInstance(_inventoryScreeen).AsSingle();
            Container.BindInstance(_gameInventoryView).AsSingle();
            Container.BindInstance(_tabSwitcher).AsSingle();

            Container.BindInstance(_rouletteScreen).AsSingle();
            Container.BindInstance(_rouletteView).AsSingle();
            Container.Bind<Roulette.Roulette>().AsSingle().WithArguments(_rouletteConfig);
            Container.BindInterfacesAndSelfTo<RouletteScreenPresentrer>().AsSingle().WithArguments(_rouletteConfig);

            Container.Bind<TopView>().FromInstance(_topView).AsSingle();
            Container.Bind<SettingsView>().FromInstance(_settingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<TopViewPrezentor>().AsSingle().WithArguments(_states);
        }
    }
}