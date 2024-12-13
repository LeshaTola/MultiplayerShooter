using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Timer;
using App.Scripts.Scenes.Gameplay.Weapons;
using Cysharp.Threading.Tasks;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class InitialState : State
    {
        private PlayerProvider _playerProvider;
        private HitService _hitService;
        private TimerProvider _timerProvider;
        private IInitializeService _initializeService;
        private HealthBarUI _healthBarUI;
        private WeaponView _weaponView;

        public InitialState(PlayerProvider playerProvider,
            HitService hitService,
            TimerProvider timerProvider,
            IInitializeService initializeService,
            HealthBarUI healthBarUI,
            WeaponView weaponView) 
        {
            _playerProvider = playerProvider;
            _hitService = hitService;
            _timerProvider = timerProvider;
            _initializeService = initializeService;
            _healthBarUI = healthBarUI;
            _weaponView = weaponView;
        }

        public override async UniTask Enter()
        {
            _initializeService.Initialize();
            _playerProvider.CreatePlayer();
            _timerProvider.Initialize();

            _healthBarUI.Initialize(_playerProvider.Player.Health);
            _weaponView.Initialize(_playerProvider.Player.WeaponProvider);

            _timerProvider.OnTimerExpired += OnTimerExpired;

            await StateMachine.ChangeState<RespawnState>();
        }

        private void OnTimerExpired()
        {
            PhotonNetwork.LeaveRoom();
            _timerProvider.OnTimerExpired -= OnTimerExpired;
        }
    }
}