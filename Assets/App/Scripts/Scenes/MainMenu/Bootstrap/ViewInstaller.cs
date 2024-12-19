using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Scenes.MainMenu.Inventory;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class ViewInstaller : MonoInstaller
    {
        [Header("Main")]
        [SerializeField] private MainScreen _mainScreen;
        [Header("Rooms")]
        [SerializeField] private RoomsView _roomsView;
        [Header("Inventory")]
        [SerializeField] private InventoryScreeen _inventoryScreeen;
        [SerializeField] private GameInventoryView _gameInventoryView;
        
        [SerializeField] private TabSwitcher _tabSwitcher;
        [SerializeField] private InventoryTab _weaponTab;
        [SerializeField] private InventoryTab _equipmentTab;
        [SerializeField] private RectTransform _overlayContainer;
        [SerializeField] private InventorySlot _slotTemplate;
        [SerializeField] private Item _itemTemplate;
        

        public override void InstallBindings()
        {
            Container.Bind<MainScreenPresenter>().AsSingle();
            Container.BindInstance(_mainScreen).AsSingle();

            Container.Bind<RoomsViewPresenter>().AsSingle();
            Container.BindInstance(_roomsView).AsSingle();


            BindSlotFactory();
            BindItemFactory();
            Container.Bind<InventoryScreenPresenter>().AsSingle();
            Container.Bind<GameInventoryViewPresenter>().AsSingle().WithArguments(_overlayContainer);
            Container.Bind<InventoryTabPresenter>().To<WeaponTabPresenter>().AsSingle().WithArguments(_overlayContainer,_weaponTab);
            Container.Bind<InventoryTabPresenter>().To<EquipmentTabPresenter>().AsSingle().WithArguments(_overlayContainer, _equipmentTab);
            Container.BindInstance(_inventoryScreeen).AsSingle();
            Container.BindInstance(_gameInventoryView).AsSingle();
            Container.BindInstance(_tabSwitcher).AsSingle();
        }

        private void BindItemFactory()
        {
            Container.Bind<Modules.Factories.IFactory<Item>>()
                .To<MonoFactory<Item>>()
                .AsSingle()
                .WithArguments(_itemTemplate);
        }

        private void BindSlotFactory()
        {
            Container
                .Bind<Modules.Factories.IFactory<InventorySlot>>()
                .To<MonoFactory<InventorySlot>>()
                .AsSingle()
                .WithArguments(_slotTemplate);
        }
    }
}