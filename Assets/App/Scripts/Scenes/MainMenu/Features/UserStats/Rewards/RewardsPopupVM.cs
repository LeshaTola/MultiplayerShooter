using System.Collections.Generic;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.UserStats;
using App.Scripts.Modules.Localization;
using App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards.Configs;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopupVM
    {
        public List<RewardConfig> Rewards { get; }
        public int LevelUps { get; }
        public float ExpValue { get; }
        
        public ILocalizationSystem LocalizationSystem { get; }
        public UserRankProvider RankProvider { get; }

        public RewardsPopupVM(ILocalizationSystem localizationSystem,
            UserRankProvider userRankProvider,
            List<RewardConfig> rewards,
            int levelUps,
            float expValue)
        {
            Rewards = rewards;
            LevelUps = levelUps;
            ExpValue = expValue;
            RankProvider = userRankProvider;
            LocalizationSystem = localizationSystem;
        }
    }
}