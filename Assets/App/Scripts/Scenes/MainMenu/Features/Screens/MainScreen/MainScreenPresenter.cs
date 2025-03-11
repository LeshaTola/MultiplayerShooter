using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using RedefineYG;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen
{
    public class MainScreenPresenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private const int MIN_NAME_LENGTH = 3;
        private const int MAX_NAME_LENGTH = 10;

        private readonly MainScreen _screen;
        private readonly ConnectionProvider _connectionProvider;
        private readonly UserRankProvider _userRankProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly TicketsProvider _ticketsProvider;
        private readonly UserStatsView _userStatsView;
        private readonly RouletteScreenPresentrer _rouletteScreenPresentrer;
        private readonly BattlePassScreenPrezenter _battlePassScreenPrezenter;
        private readonly InfoPopupRouter _infoPopupRouter;

        public MainScreenPresenter(MainScreen screen,
            ConnectionProvider connectionProvider,
            UserRankProvider userRankProvider,
            CoinsProvider coinsProvider,
            TicketsProvider ticketsProvider,
            UserStatsView userStatsView,
            RouletteScreenPresentrer rouletteScreenPresentrer,
            BattlePassScreenPrezenter battlePassScreenPrezenter,
            InfoPopupRouter infoPopupRouter)
        {
            _screen = screen;
            _connectionProvider = connectionProvider;
            _userRankProvider = userRankProvider;
            _coinsProvider = coinsProvider;
            _ticketsProvider = ticketsProvider;
            _userStatsView = userStatsView;
            _rouletteScreenPresentrer = rouletteScreenPresentrer;
            _battlePassScreenPrezenter = battlePassScreenPrezenter;
            _infoPopupRouter = infoPopupRouter;
        }

        public override void Initialize()
        {
            _screen.RouletteButtonAction += OnRouletteButtonAction;
            _screen.PlayButtonAction += OnPlayButtonAction;
            _screen.BattlePassButtonAction += OnBattlePassButtonAction;
            _screen.Initialize();

            _userStatsView.Initialize();
            _userRankProvider.OnExperienceChanded += OnExperienceChanded;
            _coinsProvider.OnCoinsChanged += OnCoinsChanged;
            _ticketsProvider.OnTicketsChanged += OnTicketsChanged;
            _userStatsView.OnPlayerNameChanged += OnPlayerNameChanged;
        }

        public void Setup()
        {
            _userStatsView.SetupName(PhotonNetwork.NickName);
            SetupRank();
            _userStatsView.SetupMoney(_coinsProvider.Coins);
            _screen.SetTicketsCount(_ticketsProvider.Tickets);
            _userStatsView.SetupTickets(_ticketsProvider.Tickets);
        }

        public override void Cleanup()
        {
            _screen.RouletteButtonAction -= OnRouletteButtonAction;
            _screen.PlayButtonAction -= OnPlayButtonAction;
            _screen.BattlePassButtonAction -= OnBattlePassButtonAction;
            _screen.Cleanup();

            _userStatsView.Cleanup();
            _userRankProvider.OnExperienceChanded -= OnExperienceChanded;
            _userStatsView.OnPlayerNameChanged -= OnPlayerNameChanged;
            _coinsProvider.OnCoinsChanged -= OnCoinsChanged;
            _ticketsProvider.OnTicketsChanged -= OnTicketsChanged;
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
        
        private async void OnBattlePassButtonAction()
        {
            await Hide();
            await _battlePassScreenPrezenter.Show();
        }

        private void OnPlayerNameChanged(string name)
        {
            if (name.Length < MIN_NAME_LENGTH || name.Length > MAX_NAME_LENGTH)
            {
                _infoPopupRouter.ShowPopup(
                    ConstStrings.ERROR,
                    $"Длина имени должна быть между {MIN_NAME_LENGTH} и {MAX_NAME_LENGTH} символами.").Forget();
                name = name.Length < MIN_NAME_LENGTH
                    ? name.PadRight(MIN_NAME_LENGTH, '_')
                    : name.Length > MAX_NAME_LENGTH
                        ? name.Substring(0, MAX_NAME_LENGTH)
                        : name;
                _userStatsView.SetupName(name);
            }

#if YANDEX
            YG2.saves.PlayerName = name;
            YG2.SaveProgress();
#else
            PlayerPrefs.SetString(ConnectionProvider.NAME_DATA, name);
#endif
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
            
            _screen.SetupRank(_userRankProvider.CurrentRank.Sprite, _userRankProvider.CurrentRank.Name);
        }

        private void OnCoinsChanged(int coins)
        {
            _userStatsView.SetupMoney(coins);
        }

        private void OnTicketsChanged(int tickets)
        {
            _screen.SetTicketsCount(tickets);
            _userStatsView.SetupTickets(tickets);
        }
    }
}