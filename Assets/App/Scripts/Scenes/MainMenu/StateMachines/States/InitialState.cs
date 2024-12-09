using System.Collections.Generic;
using App.Scripts.Features.SceneTransitions;
using App.Scripts.Features.Screens.Providers;
using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class InitialState : State
    {
        private readonly IInitializeService _initializeService;
        private readonly ICameraSwitcher _cameraSwitcher;
        private readonly string _cameraId;
        private readonly ConnectionProvider _connectionProvider;
        private readonly List<ISavable> _savable;
        private readonly ISceneTransition _sceneTransition;

        public InitialState(
            IInitializeService initializeService,
            ISceneTransition sceneTransition,
            ICameraSwitcher cameraSwitcher,
            List<ISavable> savable, 
            string cameraId,
            ConnectionProvider connectionProvider)
        {
            _initializeService = initializeService;
            _cameraSwitcher = cameraSwitcher;
            _cameraId = cameraId;
            _connectionProvider = connectionProvider;
            _savable = savable;
            _sceneTransition = sceneTransition;
        }

        public override async UniTask Enter()
        {
            await base.Enter();
            _initializeService.Initialize();
            _cameraSwitcher.SwitchCamera(_cameraId);

            foreach (var savable in _savable)
            {
                savable.LoadState();
            }
            
            _connectionProvider.OnConnected += LoadNextState;
        }

        public override async UniTask Exit()
        {
            _connectionProvider.OnConnected -= LoadNextState;
            
            await _sceneTransition.PlayOffAsync();
        }

        private async void LoadNextState()
        {
            await StateMachine.ChangeState<MainState>();
        }
    }

    public class MainState : State
    {
        private MainScreenPresenter _mainScreenPresenter;
        
        public MainState(MainScreenPresenter mainScreenPresenter)
        {
            _mainScreenPresenter = mainScreenPresenter;
        }

        public override async UniTask Enter()
        {
            _mainScreenPresenter.Initialize();
            await _mainScreenPresenter.Show();
            _mainScreenPresenter.Setup();
        }

        public override async UniTask Exit()
        {
            _mainScreenPresenter.Cleanup();
            await _mainScreenPresenter.Hide();
        }
    }
    
    /*public class InventoryState : State
    {
        public override async UniTask Enter()
        {
        }

        public override async UniTask Exit()
        {
        }
    }

    public class ShopState : State
    {
        public override async UniTask Enter()
        {
        }

        public override async UniTask Exit()
        {
        }
    }*/

    public class RoomState : State
    {
        private readonly RoomsViewPresenter _roomsViewPresenter;

        public RoomState(RoomsViewPresenter roomsViewPresenter)
        {
            _roomsViewPresenter = roomsViewPresenter;
        }

        public override async UniTask Enter()
        {
            _roomsViewPresenter.Initialize();
            await _roomsViewPresenter.Show();
        }

        public override async UniTask Exit()
        {
            _roomsViewPresenter.Cleanup();
            await _roomsViewPresenter.Hide();
        }
    }
}