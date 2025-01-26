using System;
using App.Scripts.Features.Settings;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.Features.StateMachines.States
{
    public class GlobalInitialState : State
    {
        private readonly ConnectionProvider _connectionProvider;
        private readonly SettingsProvider _settingsProvider;

        private  bool _isValid = true;
        
        public Type NextState { get; set; }

        public GlobalInitialState(ConnectionProvider connectionProvider, SettingsProvider settingsProvider)
        {
            _connectionProvider = connectionProvider;
            _settingsProvider = settingsProvider;
        }

        public override async UniTask Enter()
        {
            if (!_isValid)
            {
                await StateMachine.ChangeState(NextState);
                return;
            }
            
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name != "MainMenu")
            {
                SceneManager.LoadScene("MainMenu");
            }
            
            Application.targetFrameRate = -1;
            _connectionProvider.OnConnectionFinished += OnConectedToServer;
            
            _settingsProvider.LoadState();
            
            _isValid = false;
            _connectionProvider.Connect();
        }

        public override UniTask Exit()
        {
            _connectionProvider.OnConnectionFinished -= OnConectedToServer;
            return UniTask.CompletedTask;
        }

        private async void OnConectedToServer()
        {
             await StateMachine.ChangeState(NextState);
        }
    }
}