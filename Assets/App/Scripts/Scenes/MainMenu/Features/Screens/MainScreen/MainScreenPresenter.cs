using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen
{
    public class MainScreenPresenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private readonly MainScreen _screen;
        private readonly ConnectionProvider _connectionProvider;
        private readonly UserRankProvider _userRankProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly TicketsProvider _ticketsProvider;
        private readonly UserStatsView _userStatsView;
        private readonly RouletteScreenPresentrer _rouletteScreenPresentrer;

        public MainScreenPresenter(MainScreen screen,
            ConnectionProvider connectionProvider,
            UserRankProvider userRankProvider,
            CoinsProvider coinsProvider,
            TicketsProvider ticketsProvider,
            UserStatsView userStatsView,
            RouletteScreenPresentrer rouletteScreenPresentrer)
        {
            _screen = screen;
            _connectionProvider = connectionProvider;
            _userRankProvider = userRankProvider;
            _coinsProvider = coinsProvider;
            _ticketsProvider = ticketsProvider;
            _userStatsView = userStatsView;
            _rouletteScreenPresentrer = rouletteScreenPresentrer;
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
            await Hide();
            await _rouletteScreenPresentrer.Show();
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
            _userStatsView.SetupRank(
                rank.Name,
                rank.Sprite,
                _userRankProvider.Experience,
                _userRankProvider.CurrentRank.ExpForRank);
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