using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using App.Scripts.Scenes.MainMenu.UserProfile;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Screens.MainScreen
{
    public class MainScreenPresenter : GameScreenPresenter
    {
        private readonly MainScreen _screen;
        private readonly StateMachine _stateMachine;
        private readonly ConnectionProvider _connectionProvider;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly UserStatsView _userStatsView;

        public MainScreenPresenter(MainScreen screen,
            StateMachine stateMachine, 
            ConnectionProvider connectionProvider,
            UserStatsProvider userStatsProvider,
            UserStatsView userStatsView)
        {
            _screen = screen;
            _stateMachine = stateMachine;
            _connectionProvider = connectionProvider;
            _userStatsProvider = userStatsProvider;
            _userStatsView = userStatsView;
        }

        public override void Initialize()
        {
            _screen.PlayButtonAction += OnPlayButtonAction;
            _screen.Initialize();
            
            _userStatsView.Initialize();
            _userStatsView.OnPlayerNameChanged += OnPlayerNameChanged;
        }

        public void Setup()
        {
            _userStatsView.Setup(PhotonNetwork.NickName);
            
            var rank = _userStatsProvider.CurrentRank;
            var normalizedExp =_userStatsProvider.Experience/ rank.ExpForRank;
            _userStatsView.SetupRank(rank.Name, rank.Sprite, normalizedExp);
        }

        public override void Cleanup()
        {
            _screen.PlayButtonAction -= OnPlayButtonAction;
            _screen.Cleanup();
            
            _userStatsView.Cleanup();
            _userStatsView.OnPlayerNameChanged -= OnPlayerNameChanged;
        }

        public override async UniTask Show()
        {
            var tasks = new List<UniTask>
            {
                _userStatsView.Show(),
                _screen.Show()
            };
            await UniTask.WhenAll(tasks);
        }

        public override async UniTask Hide()
        {
            var tasks = new List<UniTask>
            {
                _userStatsView.Hide(),
                _screen.Hide()
            };
            await UniTask.WhenAll(tasks);
        }

        private void OnPlayButtonAction()
        {
            _connectionProvider.QuickGame();
        }

        private void OnPlayerNameChanged(string name)
        {
            PlayerPrefs.SetString(ConnectionProvider.NAME_DATA, name);
            PhotonNetwork.NickName = name;
        }
    }
}