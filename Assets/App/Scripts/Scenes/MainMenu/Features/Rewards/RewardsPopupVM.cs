using System.Collections.Generic;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Features.UserStats;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.MinMaxValue;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopupVM
    {
        public List<ExpAnimationData> AnimationDatas { get; }
        public List<RewardConfig> Rewards { get; }
        
        public ILocalizationSystem LocalizationSystem { get; }

        public RewardsPopupVM(ILocalizationSystem localizationSystem,
            List<RewardConfig> rewards,
            List<ExpAnimationData> animationDatas)
        {
            Rewards = rewards;
            LocalizationSystem = localizationSystem;
            AnimationDatas = animationDatas;
        }
    }
}