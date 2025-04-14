using System;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Features.Yandex.Advertisement;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Timer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RespawnState : State
    {
        private readonly PlayerProvider _playerProvider;
        private readonly PlayerController _playerController;
        private readonly GameConfig _gameConfig;
        private readonly RespawnView _respawnView;
        private readonly AdvertisementProvider _advertisementProvider;
        private readonly Modules.Timer _timer;
        
        public RespawnState(PlayerProvider playerProvider,
            PlayerController playerController,
            GameConfig gameConfig,
            RespawnView respawnView,
            AdvertisementProvider advertisementProvider)
        {
            _playerProvider = playerProvider;
            _playerController = playerController;
            _gameConfig = gameConfig;
            _respawnView = respawnView;
            _advertisementProvider = advertisementProvider;
            _timer = new();
        }

        public override async UniTask Enter()
        {
            Debug.Log("Respawn");
            _playerController.IsBusy = true;
            await _respawnView.Show();
            //await UniTask.Delay(50);
            _advertisementProvider.ShowInterstitialAd();
            
            _respawnView.ShowTimerText();
            await _timer.StartTimer(_gameConfig.RespawnTime, _respawnView.UpdateTimer);
            _respawnView.ShowPressButtonText();

            await UniTask.WaitUntil(()=>Input.anyKeyDown);
            await _respawnView.Hide();
            _playerProvider.RespawnPlayer();
            
            _playerController.IsBusy = false;
            await StateMachine.ChangeState<GameplayState>();
        }
    }
}