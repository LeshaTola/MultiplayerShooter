using System;
using App.Scripts.Features.Settings;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Modules.TasksSystem.Providers;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.Features.StateMachines.States
{
    public class GlobalInitialState : State
    {
        private readonly ConnectionProvider _connectionProvider;
        private readonly SettingsProvider _settingsProvider;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly TasksProvider _tasksProvider;

        private  bool _isValid = true;
        
        public Type NextState { get; set; }

        public GlobalInitialState(ConnectionProvider connectionProvider,
            SettingsProvider settingsProvider,
            UserStatsProvider userStatsProvider,
            TasksProvider tasksProvider)
        {
            _connectionProvider = connectionProvider;
            _settingsProvider = settingsProvider;
            _userStatsProvider = userStatsProvider;
            _tasksProvider = tasksProvider;
        }

        public override async UniTask Enter()
        {
            if (!_isValid)
            {
                await StateMachine.ChangeState(NextState);
                return;
            }
            
            ChangeToCorrectScene();
            SetTargetFPS();
            ConnectAnalytics();
            LoadSaves();

            _isValid = false;
            
            _connectionProvider.OnConnectionFinished += OnConnectedToServer;
            _connectionProvider.Connect();
        }

        private void ConnectAnalytics()
        {
            GameAnalytics.Initialize();
        }

        public override UniTask Exit()
        {
            _connectionProvider.OnConnectionFinished -= OnConnectedToServer;
            return UniTask.CompletedTask;
        }

        private void LoadSaves()
        {
            _settingsProvider.LoadState();
            _userStatsProvider.LoadState();
            _tasksProvider.FillTasks();
        }

        private static void SetTargetFPS()
        {
            Application.targetFrameRate = 60;
        }

        private static void ChangeToCorrectScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name != "MainMenu")
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        private async void OnConnectedToServer()
        {
             await StateMachine.ChangeState(NextState);
        }
    }
}