using System.Collections.Generic;
using App.Scripts.Features.SceneTransitions;
using App.Scripts.Features.Screens.Providers;
using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class InitialState : State
    {
        private readonly IInitializeService _initializeService;
        private readonly ICameraSwitcher _cameraSwitcher;
        private readonly string _cameraId;
        private readonly List<ISavable> _savable;
        private readonly ISceneTransition _sceneTransition;

        public InitialState(
            IInitializeService initializeService,
            ISceneTransition sceneTransition,
            ICameraSwitcher cameraSwitcher,
            List<ISavable> savable,
            string cameraId)
        {
            _initializeService = initializeService;
            _cameraSwitcher = cameraSwitcher;
            _cameraId = cameraId;
            _savable = savable;
            _sceneTransition = sceneTransition;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            if (PhotonNetwork.InRoom)
            {
                Debug.Log("In room");
                PhotonNetwork.LeaveRoom();
            }
            
            _initializeService.Initialize();
            _cameraSwitcher.SwitchCamera(_cameraId);

            foreach (var savable in _savable)
            {
                savable.LoadState();
            }

            await StateMachine.ChangeState<MainState>();
        }

        public override UniTask Exit()
        {
            _sceneTransition.PlayOffAsync().Forget();
            return UniTask.CompletedTask;
        }
    }
}