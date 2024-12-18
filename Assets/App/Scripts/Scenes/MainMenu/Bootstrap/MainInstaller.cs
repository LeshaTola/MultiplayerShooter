using App.Scripts.Features.Inventory;
using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.MainMenu.RoomsProviders;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private CamerasDatabase _camerasDatabase;
        [SerializeField] private RoomsProvider _roomsProvider;
        [SerializeField] private GameInventory _gameInventory;
        [SerializeField] private GlobalInventory _globalInventory;

        public override void InstallBindings()
        {
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();
            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();
            Container.Bind<ICleanupService>().To<CleanupService>().AsSingle();
            
            Container.Bind<InventoryProvider>().AsSingle().WithArguments(_gameInventory,_globalInventory);
            Container.Bind<RoomsProvider>().FromInstance(_roomsProvider);

            Container.Bind<ICameraSwitcher>().To<CameraSwitcher>().AsSingle().WithArguments(_camerasDatabase);
        }
    }
}