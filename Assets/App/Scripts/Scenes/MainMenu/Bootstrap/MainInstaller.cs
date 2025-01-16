using App.Scripts.Features.Inventory;
using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.MainMenu.Inventory;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.RoomsProviders;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private CamerasDatabase _camerasDatabase;
        [SerializeField] private RoomsProvider _roomsProvider;
        
        [SerializeField] private InventorySlot _slotTemplate;
        [SerializeField] private Item _itemTemplate;

        public override void InstallBindings()
        {
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();
            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();
            Container.Bind<ICleanupService>().To<CleanupService>().AsSingle();
            
            Container.Bind<RoomsProvider>().FromInstance(_roomsProvider);

            Container.Bind<ICameraSwitcher>().To<CameraSwitcher>().AsSingle().WithArguments(_camerasDatabase);
            
            
            BindSlotFactory();
            BindItemFactory();
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