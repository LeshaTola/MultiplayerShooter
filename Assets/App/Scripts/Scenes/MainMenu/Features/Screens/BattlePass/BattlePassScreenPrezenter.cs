using System.Linq;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass
{
    public class BattlePassScreenPrezenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly BattlePassScreen _screen;
        private readonly UserRankProvider _userRankProvider;
        private readonly ILocalizationSystem _localizationSystem;

        public BattlePassScreenPrezenter(BattlePassScreen screen,
            UserRankProvider userRankProvider, ILocalizationSystem localizationSystem)
        {
            _screen = screen;
            _userRankProvider = userRankProvider;
            _localizationSystem = localizationSystem;
        }

        public override void Initialize()
        {
            _screen.Initialize(_localizationSystem);
            _screen.SetupRewards(_userRankProvider);
            _screen.SetupRewardInfo(_userRankProvider.CurrentRank.Rewards.FirstOrDefault());
            
            _screen.SetupRankInfo(_userRankProvider.CurrentRank, _userRankProvider.Experience);

            _screen.OnRewardSelected += SelectReward;
        }

        public override void Cleanup()
        {
            _screen.Cleanup();

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

        private void SelectReward(int id)
        {
            _screen.SetupRewardInfo(_userRankProvider.RanksDatabase.Ranks[id].Rewards.FirstOrDefault());
        }
    }
}