﻿using App.Scripts.Features.SceneTransitions;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Timer;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class InitialState : State
    {
        private readonly IInitializeService _initializeService;
        private readonly PlayerController _playerController;
        private readonly PlayerProvider _playerProvider;
        private readonly TimerProvider _timerProvider;
        private readonly HealthBarUI _healthBarUI;
        private readonly WeaponView _weaponView;
        private readonly ISceneTransition _sceneTransition;

        public InitialState(PlayerProvider playerProvider,
            TimerProvider timerProvider,
            IInitializeService initializeService,
            PlayerController playerController,
            HealthBarUI healthBarUI,
            WeaponView weaponView,
            ISceneTransition sceneTransition) 
        {
            _playerProvider = playerProvider;
            _timerProvider = timerProvider;
            _initializeService = initializeService;
            _playerController = playerController;
            _healthBarUI = healthBarUI;
            _weaponView = weaponView;
            _sceneTransition = sceneTransition;
        }

        public override async UniTask Enter()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            Debug.Log("Initial");
            _initializeService.Initialize();
            
            await UniTask.WaitUntil(() => PhotonNetwork.Time != 0);
            _timerProvider.Initialize();
            _timerProvider.OnTimerExpired += OnTimerExpired;
            
            _playerController.Setup(_playerProvider.Player);

            _healthBarUI.Initialize(_playerProvider.Player.Health);
            _weaponView.Initialize(_playerProvider.Player.WeaponProvider);
            
            await StateMachine.ChangeState<RespawnState>();
        }

        private async void OnTimerExpired()
        {
            await StateMachine.ChangeState<EndGame>();
        }

        public override async UniTask Exit()
        {
            await base.Exit();
            await _sceneTransition.PlayOffAsync();
        }
    }
}