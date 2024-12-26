using System;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Timer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RespawnState : State
    {
        private PlayerProvider _playerProvider;
        private GameConfig _gameConfig;
        private readonly RespawnView _respawnView;
        private Modules.Timer _timer;
        
        public RespawnState(PlayerProvider playerProvider, GameConfig gameConfig, RespawnView respawnView)
        {
            _playerProvider = playerProvider;
            _gameConfig = gameConfig;
            _respawnView = respawnView;
            _timer = new();
        }

        public override async UniTask Enter()
        {
            Debug.Log("Respawn");

            await _respawnView.Show();
            _respawnView.ShowTimerText();
            await _timer.StartTimer(_gameConfig.RespawnTime, _respawnView.UpdateTimer);
            _respawnView.ShowPressButtonText();

            await UniTask.WaitUntil(()=>Input.anyKeyDown);
            await _respawnView.Hide();
            
            _playerProvider.RespawnPlayer();
            await StateMachine.ChangeState<GameplayState>();
        }
    }
}