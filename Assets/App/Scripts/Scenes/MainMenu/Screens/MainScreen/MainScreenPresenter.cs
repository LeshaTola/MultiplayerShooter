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
            _screen.OnPlayerNameChanged += OnPlayerNameChanged;
            _screen.Initialize();
            _userStatsView.Initialize();
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
        }

        public override async UniTask Show()
        {
            _userStatsView.Show();
            await _screen.Show();
        }

        public override async UniTask Hide()
        {
            _userStatsView.Hide();
            await _screen.Hide();
        }

        private void OnPlayButtonAction()
        {
            _connectionProvider.QuickGame();
            //await _stateMachine.ChangeState<RoomState>();
        }

        private async void OnInventoryButtonAction()
        {
            await _stateMachine.ChangeState<InventoryState>();
        }

        private void OnPlayerNameChanged(string name)
        {
            PlayerPrefs.SetString(ConnectionProvider.NAME_DATA, name);
            PhotonNetwork.NickName = name;
        }
    }
}