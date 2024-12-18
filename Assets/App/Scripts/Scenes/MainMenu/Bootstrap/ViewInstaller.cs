using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Scenes.MainMenu.Inventory;
using App.Scripts.Scenes.MainMenu.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private MainScreen _mainScreen;
        [SerializeField] private RoomsView _roomsView;
        [SerializeField] private InventoryScreeen _inventoryScreeen;
        [SerializeField] private TabSwitcher _tabSwitcher;
        [SerializeField] private InventoryTab _inventoryTab;
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
            Container.Bind<InventoryTabPresenter>().AsSingle().WithArguments(_overlayContainer);
            Container.BindInstance(_inventoryScreeen).AsSingle();
            Container.BindInstance(_tabSwitcher).AsSingle();
            Container.BindInstance(_inventoryTab).AsSingle();
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