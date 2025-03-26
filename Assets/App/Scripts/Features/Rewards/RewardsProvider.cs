using System.Linq;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Features.UserStats.Rank;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Timer;
using GameAnalyticsSDK;
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
        private readonly MapsProvider _mapsProvider;

        private float _experience;
        private float _coins;

        public static RewardsProvider Instance { get; private set; }

        public RewardsProvider(RewardService rewardService,
            LeaderBoardProvider leaderboard,
            AccrualConfig accrualConfig,
            TimerProvider timerProvider,
            UserRankProvider rankProvider, 
            MapsProvider mapsProvider)
        {
            _rewardService = rewardService;
            _leaderboard = leaderboard;
            _accrualConfig = accrualConfig;
            _timerProvider = timerProvider;
            _rankProvider = rankProvider;
            _mapsProvider = mapsProvider;
            Instance = this;
        }

        public void ApplyEndMatchRewards()
        {
            if (!IsMinimumTime())
            {
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                GameAnalytics.NewDesignEvent($"game:map:{_mapsProvider.CurrentMap.name.ToLower()}", 1);
                var kills = _leaderboard.GetTable().Sum(x => x.Item3);
                GameAnalytics.NewDesignEvent("game:kills:", kills);
                GameAnalytics.NewDesignEvent("game:players:", _leaderboard.GetTable().Count);
            }

            AddExpFromPlace();
            ApplyCoins();
            ApplyExp();
        }

        public void ApplyKill()
        {
            var growsCount = (int) ((PhotonNetwork.Time - _timerProvider.LocalStartTime) / _accrualConfig.GrowExpTime);
            var exp = _accrualConfig.ExpPerKill + growsCount * _accrualConfig.GrowExpValue;
            Debug.Log($"EXP FOR KILL: {exp}");
            _experience += exp;

            var coins = _accrualConfig.CoinsPerKill;
            _coins += coins;

            ApplyCoins();
            ApplyExp();
        }

        private void ApplyExp()
        {
            _rewardService.ExperienceToAdd += (int) _experience;
            Debug.Log((int) _experience);
            _experience -= (int) _experience;
        }

        private void ApplyCoins()
        {
            if (_coins <= 1)
            {
                return;
            }

            Debug.Log((int) _coins);

            var coinsReward = Object.Instantiate(_accrualConfig.CoinReward);
            coinsReward.Count = (int) _coins;
            _coins -= (int) _coins;

            _rewardService.AddReward(coinsReward);
        }

        private void AddExpFromPlace()
        {
            int playersCount = _leaderboard.GetTable().Count;
            int middlePlace = playersCount / 2;
            if (_leaderboard.MyPlace <= middlePlace)
            {
                var maxExpPercent = _accrualConfig.MaxExpPercent * playersCount / 10;
                var myPlaceDivider = (_leaderboard.MyPlace - 1) * 2;
                myPlaceDivider = myPlaceDivider == 0 ? 1 : myPlaceDivider;
                var exp = maxExpPercent / myPlaceDivider * _rankProvider.CurrentRank.ExpForRank / 100;
                Debug.Log($"EXP FOR PLACE: {exp}");
                _experience += exp;
            }
        }

        private bool IsMinimumTime()
        {
            return PhotonNetwork.Time - _timerProvider.LocalStartTime > _accrualConfig.MinMatchTime;
        }
    }
}