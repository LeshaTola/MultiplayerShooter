using System.Collections.Generic;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.MinMaxValue;
using App.Scripts.Modules.Sounds.Providers;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopupVM
    {
        public List<ExpAnimationData> AnimationDatas { get; }
        public List<RewardConfig> Rewards { get; }
        
        public ILocalizationSystem LocalizationSystem { get; }
        public ISoundProvider SoundProvider { get; set; }

        public RewardsPopupVM(ILocalizationSystem localizationSystem,
            List<RewardConfig> rewards,
            List<ExpAnimationData> animationDatas, ISoundProvider soundProvider)
        {
            Rewards = rewards;
            LocalizationSystem = localizationSystem;
            AnimationDatas = animationDatas;
            SoundProvider = soundProvider;
        }
    }
}