using System.Linq;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Features.UserStats.Rank;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass
{
    public class BattlePassScreenPrezenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private readonly BattlePassScreen _screen;
        private readonly UserRankProvider _userRankProvider;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly ISoundProvider _soundProvider;

        public BattlePassScreenPrezenter(BattlePassScreen screen,
            UserRankProvider userRankProvider, ILocalizationSystem localizationSystem, ISoundProvider soundProvider)
        {
            _screen = screen;
            _userRankProvider = userRankProvider;
            _localizationSystem = localizationSystem;
            _soundProvider = soundProvider;
        }

        public override void Initialize()
        {
            _screen.Initialize(_localizationSystem);
            _screen.SetupRewards(_userRankProvider);
            _screen.SetupRewardInfo(_userRankProvider.CurrentRank.Rewards.FirstOrDefault());

            UpdateRank();
            _userRankProvider.OnExperienceChanded += UpdateRank;
            _screen.OnRewardSelected += SelectReward;
        }

        public override void Cleanup()
        {
            _screen.Cleanup();

            _userRankProvider.OnExperienceChanded -= UpdateRank;
            _screen.OnRewardSelected -= SelectReward;
        }

        public override async UniTask Show()
        {
            await _screen.Show();
        }

        public override async UniTask Hide()
        {
            await _screen.Hide();
        }

        private void UpdateRank()
        {
            _screen.SetupRankInfo(_userRankProvider.CurrentRank, _userRankProvider.Experience);
            _screen.UpdateSlider(_userRankProvider);
        }

        private void SelectReward(int id)
        {
            _screen.SetupRewardInfo(_userRankProvider.RanksDatabase.Ranks[id].Rewards.FirstOrDefault());
            _soundProvider.PlayOneShotSound(_screen.ClickSond);
        }
    }
}