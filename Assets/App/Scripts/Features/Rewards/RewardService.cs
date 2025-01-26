using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Features.UserStats
{
    public class RewardService
    {
        private readonly RewardsPopupRouter _popupRouter;
        private readonly UserRankProvider _rankProvider;
        private readonly InventoryProvider _inventoryProvider;
        private readonly CoinsProvider _coinsProvider;
        private readonly TicketsProvider _ticketsProvider;
        private readonly List<RewardConfig> _rewards = new();

        public int ExperienceToAdd { get; set; }

        public RewardService(RewardsPopupRouter popupRouter,
            UserRankProvider rankProvider,
            InventoryProvider inventoryProvider,
            CoinsProvider coinsProvider,
            TicketsProvider ticketsProvider)
        {
            _popupRouter = popupRouter;
            _rankProvider = rankProvider;
            _inventoryProvider = inventoryProvider;
            _coinsProvider = coinsProvider;
            _ticketsProvider = ticketsProvider;
        }

        public void AddReward(RewardConfig rewardConfig)
        {
            var reward = _rewards.FirstOrDefault(x => x.Reward.Id.Equals(rewardConfig.Reward.Id));
            if (!reward)
            {
                _rewards.Add(rewardConfig);
                return;
            }

            reward.Count += rewardConfig.Count;
        }

        public bool HasAnyReward()
        {
            return _rewards.Count > 0 || ExperienceToAdd > 0;
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
            var levelUps = _rankProvider.AddExperience(ExperienceToAdd);
            ExperienceToAdd = 0;
            var expValue = (float) _rankProvider.Experience / _rankProvider.CurrentRank.ExpForRank;

            var rewards = _rewards.ToList();
            ApplyRewards(_rewards);

            _rewards.Clear();
            await _popupRouter.ShowPopup(rewards, levelUps, expValue);
        }

        private void ApplyRewards(List<RewardConfig> rewardConfigs)
        {
            foreach (var rewardConfig in rewardConfigs)
            {
                switch (rewardConfig.Reward)
                {
                    case WeaponConfig weaponConfig:
                        _inventoryProvider.Inventory.Weapons.Add(weaponConfig.Id);
                        break;
                    case EquipmentConfig equipmentConfig:
                        _inventoryProvider.Inventory.Equipment.Add(equipmentConfig.Id);
                        break;
                    case SkinConfig skinConfig:
                        _inventoryProvider.Inventory.Skins.Add(skinConfig.Id);
                        break;
                    case OtherItemConfig otherItemConfig:
                        switch (otherItemConfig.ItemType)
                        {
                            case OtherItemType.Coin:
                                _coinsProvider.ChangeCoins(rewardConfig.Count);
                                break;
                            case OtherItemType.Ticket:
                                _ticketsProvider.ChangeTickets(rewardConfig.Count);
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
}