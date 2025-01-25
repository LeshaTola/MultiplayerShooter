using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen
{
    public class MainScreenPresenter : GameScreenPresenter
    {
        private readonly MainScreen _screen;
        private readonly StateMachine _stateMachine;
        private readonly ConnectionProvider _connectionProvider;
        private readonly UserRankProvider _userRankProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly TicketsProvider _ticketsProvider;
        private readonly UserStatsView _userStatsView;

        public MainScreenPresenter(MainScreen screen,
            StateMachine stateMachine, 
            ConnectionProvider connectionProvider,
            UserRankProvider userRankProvider,
            CoinsProvider coinsProvider,
            TicketsProvider ticketsProvider,
            UserStatsView userStatsView)
        {
            _screen = screen;
            _stateMachine = stateMachine;
            _connectionProvider = connectionProvider;
            _userRankProvider = userRankProvider;
            _coinsProvider = coinsProvider;
            _ticketsProvider = ticketsProvider;
            _userStatsView = userStatsView;
        }

        public override void Initialize()
        {
            _screen.RouletteButtonAction += OnRouletteButtonAction;
            _screen.PlayButtonAction += OnPlayButtonAction;
            _screen.Initialize();
            
            _userStatsView.Initialize();
            _userRankProvider.OnExperienceChanded += OnExperienceChanded;
            _coinsProvider.OnCoinsChanged += OnCoinsChanged;
            _ticketsProvider.OnTicketsChanged += OnTicketsChanged;
            _userStatsView.OnPlayerNameChanged += OnPlayerNameChanged;
        }

        public void Setup()
        {
            _userStatsView.Setup(PhotonNetwork.NickName);
            SetupRank();
            _userStatsView.SetupMoney(_coinsProvider.Coins);
            _screen.SetTicketsCount(_ticketsProvider.Tickets);
        }

        public override void Cleanup()
        {
            _screen.RouletteButtonAction -= OnRouletteButtonAction;
            _screen.PlayButtonAction -= OnPlayButtonAction;
            _screen.Cleanup();
            
            _userStatsView.Cleanup();
            _userRankProvider.OnExperienceChanded -= OnExperienceChanded;
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
        
        private async void OnRouletteButtonAction()
        {
            await _stateMachine.ChangeState<RouletteState>();
        }
        
        private void OnPlayerNameChanged(string name)
        {
            PlayerPrefs.SetString(ConnectionProvider.NAME_DATA, name);
            PhotonNetwork.NickName = name;
        }

        private void OnExperienceChanded()
        {
            SetupRank();
        }

        private void SetupRank()
        {
            var rank = _userRankProvider.CurrentRank;
            var normalizedExp =_userRankProvider.Experience/ rank.ExpForRank;
            _userStatsView.SetupRank(rank.Name, rank.Sprite, normalizedExp);
        }

        private void OnCoinsChanged(int coins)
        {
            _userStatsView.SetupMoney(coins);
        }
        
        private void OnTicketsChanged(int tickets)
        {
            _screen.SetTicketsCount(tickets);
        }
    }
}