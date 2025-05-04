using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.Commands;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Features.UserStats.Rank;
using App.Scripts.Features.Yandex.Advertisement;
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
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen
{
    public class MainScreenPresenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        public const string NAME_DATA = "playerName";
        private const int MIN_NAME_LENGTH = 3;
        private const int MAX_NAME_LENGTH = 20;

        private readonly MainScreen _screen;
        private readonly ConnectionProvider _connectionProvider;
        private readonly UserRankProvider _userRankProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly TicketsProvider _ticketsProvider;
        private readonly UserStatsView _userStatsView;
        private readonly TopView _topView;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly AdvertisementProvider _advertisementProvider;
        private readonly InputFieldPopupRouter _inputFieldPopupRouter;

        public MainScreenPresenter(MainScreen screen,
            ConnectionProvider connectionProvider,
            UserRankProvider userRankProvider,
            CoinsProvider coinsProvider,
            TicketsProvider ticketsProvider,
            UserStatsView userStatsView,
            InfoPopupRouter infoPopupRouter, 
            TopView topView,
            AdvertisementProvider advertisementProvider,
            InputFieldPopupRouter inputFieldPopupRouter)
        {
            _screen = screen;
            _connectionProvider = connectionProvider;
            _userRankProvider = userRankProvider;
            _coinsProvider = coinsProvider;
            _ticketsProvider = ticketsProvider;
            _userStatsView = userStatsView;
            _infoPopupRouter = infoPopupRouter;
            _topView = topView;
            _advertisementProvider = advertisementProvider;
            _inputFieldPopupRouter = inputFieldPopupRouter;
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
            LoadPlayrName();
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
            _advertisementProvider.ShowInterstitialAd();
            _connectionProvider.QuickGame();
        }

        private async void OnRouletteButtonAction()
        {
            await Hide();
            _topView.SetTab(4);
        }

        private async void OnBattlePassButtonAction()
        {
            await Hide();
            _topView.SetTab(3);
        }

        private void OnPlayerNameChanged(string name)
        {
            if (!IsValidLength(name))
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

        private static bool IsValidLength(string name)
        {
            return name.Length >= MIN_NAME_LENGTH && name.Length <= MAX_NAME_LENGTH;
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

        private async void LoadPlayrName()
        {
#if YANDEX
            var name = YG2.player.auth ? YG2.player.name : $"Player {Random.Range(0, 1000)}";
            var playerName = !string.IsNullOrEmpty(YG2.saves.PlayerName) ? YG2.saves.PlayerName : name;
            
            if (string.IsNullOrEmpty(YG2.saves.PlayerName))
            {
                bool isValid;
                do
                {
                    playerName = await ShowInputFieldPopup(playerName);
                    isValid = IsValidLength(playerName);
                    if (!isValid)
                    {
                        _infoPopupRouter.ShowPopup(
                            ConstStrings.ERROR,
                            $"Длина имени должна быть между {MIN_NAME_LENGTH} и {MAX_NAME_LENGTH} символами.").Forget();
                        continue;
                    }

                    _inputFieldPopupRouter.HidePopup().Forget();
                    _userStatsView.SetupName(playerName);
                    YG2.saves.PlayerName = playerName;
                    YG2.SaveProgress();
                }
                while(!isValid);
            }
#else
            var playerName =
 PlayerPrefs.HasKey(NAME_DATA)? PlayerPrefs.GetString(NAME_DATA): $"Player {Random.Range(0, 1000)}";
            PlayerPrefs.SetString(playerName, NAME_DATA);
#endif
            PhotonNetwork.NickName = playerName;
        }

        private UniTask<string> ShowInputFieldPopup(string playerName)
        {
            return _inputFieldPopupRouter.ShowPopup(new()
            {
                Placeholder = "Введите ваше имя",
                StartValue = playerName,
                Header = "Ник игрока",
                Mesage = "Как вас зовут?",
                OnEndEdit = OnPlayerNameChanged,
                Command = new CustomCommand(ConstStrings.CONFIRM, () =>
                {
                })
            });
        }
    }
}