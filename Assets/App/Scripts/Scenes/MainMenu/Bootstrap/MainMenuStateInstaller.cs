using System.Collections.Generic;
using App.Scripts.Features.StateMachines.States;
using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.Factories.States;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class MainMenuStateInstaller : MonoInstaller
    {
        [SerializeField] private CamerasDatabase _camerasDatabase;

        [ValueDropdown(nameof(GetCamerasIds))]
        [SerializeField] private string _mainCameraId;

        [ValueDropdown(nameof(GetCamerasIds))]
        public override void InstallBindings()
        {
            BindStatesFactory();
            BindStateMachine();

            BindInitialState();
            Container.Bind<State>().To<MainState>().AsSingle();
            Container.Bind<State>().To<RouletteState>().AsSingle();
            Container.Bind<State>().To<RoomState>().AsSingle();
            Container.Bind<State>().To<InventoryState>().AsSingle();
            Container.Bind<State>().To<LoadSceneState>().AsSingle().WithArguments("Gameplay");
        }

        private void BindInitialState()
        {
            Container.Bind<State>().To<InitialState>().AsSingle()
                .WithArguments(_mainCameraId);
        }

        private void BindStateMachine()
        {
            Container.Bind<StateMachine>().AsSingle();
        }

        private void BindStatesFactory()
        {
            Container.Bind<IStatesFactory>().To<StatesFactory>().AsSingle();
        }

        public IEnumerable<string> GetCamerasIds()
        {
            if (_camerasDatabase == null)
            {
                return null;
            }

            return new List<string>(_camerasDatabase.Cameras.Keys);
        }
    }
}