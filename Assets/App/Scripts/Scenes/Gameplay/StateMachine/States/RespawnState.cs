using System.Threading.Tasks;
using App.Scripts.Features.GameMods.Providers;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Features.Yandex.Advertisement;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RespawnState : State
    {
        private readonly PlayerProvider _playerProvider;
        private readonly PlayerController _playerController;
        private readonly GameModProvider _gameModProvider;
        private readonly RespawnView _respawnView;
        private readonly AdvertisementProvider _advertisementProvider;
        private readonly Modules.Timer _timer;

        public RespawnState(PlayerProvider playerProvider,
            PlayerController playerController,
            GameModProvider gameModProvider,
            RespawnView respawnView,
            AdvertisementProvider advertisementProvider)
        {
            _playerProvider = playerProvider;
            _playerController = playerController;
            _gameModProvider = gameModProvider;
            _respawnView = respawnView;
            _advertisementProvider = advertisementProvider;
            _timer = new Modules.Timer();
        }

        public override async UniTask Enter()
        {
            Debug.Log("Respawn");
            _playerController.IsBusy = true;
            await _respawnView.Show();

            _respawnView.ShowTimerText();
            await SetRespawnTimer();
            _respawnView.ShowPressButtonText();

            await UniTask.WaitUntil(() => Input.anyKeyDown);
            await _respawnView.Hide();
            _playerProvider.RespawnPlayer();

            _playerController.IsBusy = false;
            await StateMachine.ChangeState<GameplayState>();
        }

        private async UniTask SetRespawnTimer()
        {
            await _timer.StartTimer(_gameModProvider.CurrentGameMod.GameConfig.RespawnTime, _respawnView.UpdateTimer);
        }

        public override UniTask Exit()
        {
            _advertisementProvider.ShowInterstitialAd();
            return base.Exit();
        }
    }
}