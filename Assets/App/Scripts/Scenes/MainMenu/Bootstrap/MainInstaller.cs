using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Commands;
using App.Scripts.Scenes.MainMenu.Features.Inventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private CamerasDatabase _camerasDatabase;

        [SerializeField] private InventorySlot _slotTemplate;
        [SerializeField] private Item _itemTemplate;

        [SerializeField] private Transform _weaponContainer;
        [SerializeField] private PlayerVisual _playerVisual;

        public override void InstallBindings()
        {
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();
            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();
            Container.Bind<ICleanupService>().To<CleanupService>().AsSingle();

            Container.Bind<MarketPopupRouter>().AsSingle();
            Container.Bind<WeaponModelsUIProvider>().AsSingle().WithArguments(_weaponContainer, "Weapon");
            Container.Bind<PlayerModelsUIProvider>().AsSingle().WithArguments(_playerVisual);

            Container.Bind<BuyItemCommand>().AsTransient().WithArguments("Buy", "Buy_sound");
            Container.Bind<EquipItemCommand>().AsTransient().WithArguments("Equip");
            
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