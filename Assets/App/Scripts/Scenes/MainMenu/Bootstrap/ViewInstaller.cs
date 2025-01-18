using System;
using System.Collections.Generic;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.MainMenu.Inventory;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Screens;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using App.Scripts.Scenes.MainMenu.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using App.Scripts.Scenes.MainMenu.UserProfile;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private InventoryTab _equipmentTab;
        [Space]
        [SerializeField] private InventoryTab _skinTab;
        [SerializeField] private InventorySlot _skinSlot;
        
        [Header("TopScreen")]
        [SerializeField] private TopView _topView;
        [SerializeField] private SettingsView _settingsView;
        [SerializeField, SerializeReference, OdinSerialize] private List<Type> _states = new()
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

            Container.Bind<RoomsViewPresenter>().AsSingle();
            Container.BindInstance(_roomsView).AsSingle();
            
            Container.Bind<InventoryScreenPresenter>().AsSingle();
            Container.Bind<GameInventoryViewPresenter>().AsSingle().WithArguments(_overlayContainer);
            Container.Bind<InventoryTabPresenter>().To<WeaponTabPresenter>().AsSingle().WithArguments(_overlayContainer,_weaponTab);
            Container.Bind<InventoryTabPresenter>().To<EquipmentTabPresenter>().AsSingle().WithArguments(_overlayContainer, _equipmentTab);
            Container.Bind<InventoryTabPresenter>().To<SkinsTabPresenter>().AsSingle().WithArguments(_overlayContainer, _skinTab, _skinSlot);
            Container.BindInstance(_inventoryScreeen).AsSingle();
            Container.BindInstance(_gameInventoryView).AsSingle();
            Container.BindInstance(_tabSwitcher).AsSingle();
            
            Container.Bind<TopView>().FromInstance(_topView).AsSingle();
            Container.Bind<SettingsView>().FromInstance(_settingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<TopViewPrezentor>().AsSingle().WithArguments(_states);
        }
    }
}