using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Modules.TasksSystem.CompleteActions;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.CompleteActions
{
    public class GiveRewardAction: CompleteAction
    {
        [SerializeField] private List<RewardConfig> _rewardConfigs;

        private readonly RewardService _rewardService;

        public GiveRewardAction(RewardService rewardService)
        {
            _rewardService = rewardService;
        }

        public override void Execute()
        {
            _rewardService.AddRewards(_rewardConfigs);
        }

        public override void Import(CompleteAction original)
        {
            var concrete = (GiveRewardAction) original;
            _rewardConfigs = concrete._rewardConfigs;
        }

        public override List<RewardData> GetRewardData()
        {
            return _rewardConfigs.Select(x=> new RewardData()
            {
                Sprite = x.Reward.Sprite,
                Text = x.Reward.Id,
                ValueText = x.Count.ToString(),
                Color = x.Color
            }).ToList();
        }
    }
}