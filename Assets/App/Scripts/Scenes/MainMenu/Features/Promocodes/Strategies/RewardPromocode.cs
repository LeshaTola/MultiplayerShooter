using System.Collections.Generic;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Promocodes.Strategies
{
    public class RewardPromocode : PromocodeAction
    {
        [SerializeField] private List<RewardConfig> _rewardConfigs;
        
        private readonly RewardService _rewardService;
        
        public RewardPromocode(RewardService rewardService)
        {
            _rewardService = rewardService;
        }

        public override void Execute()
        {
            _rewardService.AddRewards(_rewardConfigs);
            _rewardService.ApplyRewardsAsync().Forget();
        }

        public override void Import(PromocodeAction original)
        {
            var concrete = (RewardPromocode) original;
            _rewardConfigs = concrete._rewardConfigs;
        }
    }
}