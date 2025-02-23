using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Timer;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Rewards
{
    public class RewardsProvider
    {
        private readonly RewardService _rewardService;
        private readonly LeaderBoardProvider _leaderboard;
        private readonly AccrualConfig _accrualConfig;
        private readonly TimerProvider _timerProvider;
        private readonly UserRankProvider _rankProvider;

        private float _experience;
        private float _coins;

        public static RewardsProvider Instance { get; private set; }

        public RewardsProvider(RewardService rewardService,
            LeaderBoardProvider leaderboard,
            AccrualConfig accrualConfig,
            TimerProvider timerProvider, 
            UserRankProvider rankProvider)
        {
            _rewardService = rewardService;
            _leaderboard = leaderboard;
            _accrualConfig = accrualConfig;
            _timerProvider = timerProvider;
            _rankProvider = rankProvider;
            Instance = this;
        }

        public void ApplyEndMatchRewards()
        {
            if (PhotonNetwork.Time - _timerProvider.LocalStartTime < _accrualConfig.MinMatchTime)
            {
                return;
            }
            
            int playersCount = _leaderboard.GetTable().Count;
            int middlePlace = playersCount / 2;
            if (_leaderboard.MyPlace <= middlePlace)
            {
                var maxExpPercent = _accrualConfig.MaxExpPercent * playersCount / 10;
                var myPlaceDivider = (_leaderboard.MyPlace - 1) * 2;
                var exp =  maxExpPercent / myPlaceDivider * _rankProvider.CurrentRank.ExpForRank / 100;
                _experience += exp;
            }
            
            ApplyCoins();
            ApplyExp();
        }

        public void ApplyKill()
        {
            var growsCount = (int)((PhotonNetwork.Time - _timerProvider.LocalStartTime) / _accrualConfig.GrowExpTime);
            var exp = _accrualConfig.ExpPerKill + growsCount * _accrualConfig.GrowExpValue;
            _experience += exp;

            var coins = _accrualConfig.CoinsPerKill;
            _coins += coins;

            ApplyCoins();
            ApplyExp();
        }

        private void ApplyExp()
        {
            _rewardService.ExperienceToAdd += (int) _experience;
            _experience -= (int) _experience;
        }

        private void ApplyCoins()
        {
            if (_coins <= 1)
            {
                return;
            }

            var coinsReward = Object.Instantiate(_accrualConfig.CoinReward);
            coinsReward.Count = (int) _coins;
            _coins -= (int) _coins;

            _rewardService.AddReward(coinsReward);
        }
    }
}