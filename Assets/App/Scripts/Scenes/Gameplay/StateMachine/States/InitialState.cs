using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
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
        private PlayerProvider _playerProvider;
        private TimerProvider _timerProvider;
        private IInitializeService _initializeService;
        private readonly PlayerController _playerController;
        private HealthBarUI _healthBarUI;
        private WeaponView _weaponView;

        public InitialState(PlayerProvider playerProvider,
            TimerProvider timerProvider,
            IInitializeService initializeService,
            PlayerController playerController,
            HealthBarUI healthBarUI,
            WeaponView weaponView) 
        {
            _playerProvider = playerProvider;
            _timerProvider = timerProvider;
            _initializeService = initializeService;
            _playerController = playerController;
            _healthBarUI = healthBarUI;
            _weaponView = weaponView;
        }

        public override async UniTask Enter()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            Debug.Log("Initial");
            
            _initializeService.Initialize();
            _timerProvider.Initialize();
            
            
            _playerController.Setup(_playerProvider.Player);

            _healthBarUI.Initialize(_playerProvider.Player.Health);
            _weaponView.Initialize(_playerProvider.Player.WeaponProvider);

            _timerProvider.OnTimerExpired += OnTimerExpired;

            await StateMachine.ChangeState<RespawnState>();
        }

        private async void OnTimerExpired()
        {
            await StateMachine.ChangeState<EndGame>();
        }
    }
}