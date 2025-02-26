using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.Rewards
{
    public class RewardService
    {
        private readonly RewardsPopupRouter _popupRouter;
        private readonly UserStatsProvider _userStatsProvider;

        private readonly List<RewardConfig> _rewards = new();

        public int ExperienceToAdd { get; set; }

        public RewardService(RewardsPopupRouter popupRouter,UserStatsProvider userStatsProvider)
        {
            _popupRouter = popupRouter;
            _userStatsProvider = userStatsProvider;
        }

        public void AddRewards(List<RewardConfig> rewards)
        {
            foreach (var reward in rewards)
            {
                AddReward(reward);
            }
        }
        
        public void AddReward(RewardConfig rewardConfig)
        {
            var reward = _rewards.FirstOrDefault(x => x.Reward.Id.Equals(rewardConfig.Reward.Id));
            if (!reward)
            {
                var newRewardConfig = GameObject.Instantiate(rewardConfig);
                _rewards.Add(newRewardConfig);
                return;
            }

            reward.Count += rewardConfig.Count;
        }

        public bool HasAnyReward()
        {
            return HasRewards() || HasExperience();
        }

        public bool HasExperience()
        {
            return ExperienceToAdd > 0;
        }

        public bool HasRewards()
        {
            return _rewards.Count > 0;
        }

        public async UniTask ApplyRewardsAsync()
        {
            List<ExpAnimationData> animationDatas = new();
            ExpAnimationData startExpValue = new()
            {
                FromExp = _userStatsProvider.RankProvider.Experience,
                ToExp = _userStatsProvider.RankProvider.CurrentRank.ExpForRank,
                MaxExp = _userStatsProvider.RankProvider.CurrentRank.ExpForRank,
                FromSprite = _userStatsProvider.RankProvider.CurrentRank.Sprite,
                ToSprite = _userStatsProvider.RankProvider.NextRank.Sprite,
            };
            Debug.Log(ExperienceToAdd);
            var levelUps = _userStatsProvider.RankProvider.AddExperience(ExperienceToAdd);
            ExperienceToAdd = 0;


            if (levelUps > 0)
            {
                animationDatas.Add(startExpValue);
                while (levelUps > 1)
                {
                    levelUps--;
                    var rankId = _userStatsProvider.RankProvider.CurrentRankId - levelUps;
                    var rank = _userStatsProvider.RankProvider.RanksDatabase.Ranks[rankId];
                    var nextRank = _userStatsProvider.RankProvider.RanksDatabase.Ranks[rankId + 1];
                    ;
                    ExpAnimationData animationData = new()
                    {
                        FromExp = 0,
                        ToExp = rank.ExpForRank,
                        MaxExp = rank.ExpForRank,
                        FromSprite = rank.Sprite,
                        ToSprite = nextRank.Sprite,
                    };
                    animationDatas.Add(animationData);
                }

                ExpAnimationData endExpValue = new()
                {
                    FromExp = 0,
                    ToExp = _userStatsProvider.RankProvider.Experience,
                    MaxExp = _userStatsProvider.RankProvider.CurrentRank.ExpForRank,
                    FromSprite = _userStatsProvider.RankProvider.CurrentRank.Sprite,
                    ToSprite = _userStatsProvider.RankProvider.NextRank.Sprite,
                };
                animationDatas.Add(endExpValue);
            }
            else
            {
                startExpValue.ToExp = _userStatsProvider.RankProvider.Experience;
                animationDatas.Add(startExpValue);
            }

            var rewards = _rewards.ToList();
            ApplyRewards(_rewards);
            _userStatsProvider.SaveState();

            _rewards.Clear();
            await _popupRouter.ShowPopup(rewards, animationDatas);
            
        }

        private void ApplyRewards(List<RewardConfig> rewardConfigs)
        {
            foreach (var rewardConfig in rewardConfigs)
            {
                switch (rewardConfig.Reward)
                {
                    case WeaponConfig weaponConfig:
                        _userStatsProvider.InventoryProvider.Inventory.Weapons.Add(weaponConfig.Id);
                        _userStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
                        break;
                    case EquipmentConfig equipmentConfig:
                        _userStatsProvider.InventoryProvider.Inventory.Equipment.Add(equipmentConfig.Id);
                        _userStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
                        break;
                    case SkinConfig skinConfig:
                        _userStatsProvider.InventoryProvider.Inventory.Skins.Add(skinConfig.Id);
                        _userStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
                        break;
                    case OtherItemConfig otherItemConfig:
                        switch (otherItemConfig.ItemType)
                        {
                            case OtherItemType.Coin:
                                _userStatsProvider.CoinsProvider.ChangeCoins(rewardConfig.Count);
                                break;
                            case OtherItemType.Ticket:
                                _userStatsProvider.TicketsProvider.ChangeTickets(rewardConfig.Count);
                                break;
                            case OtherItemType.Exp:
                                ExperienceToAdd += rewardConfig.Count;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    default:
                        throw new InvalidOperationException($"Unknown reward config type: {rewardConfig.GetType()}");
                }
            }
        }
    }

    public class ExpAnimationData
    {
        public float FromExp;
        public float ToExp;
        public float MaxExp;
        public Sprite FromSprite;
        public Sprite ToSprite;
    }
}