using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Timer;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Rewards
{
    public class RewardProvider
    {
        private readonly RewardService _rewardService;
        private readonly LeaderBoardProvider _leaderboard;
        private readonly AccrualConfig _accrualConfig;
        private readonly TimerProvider _timerProvider;

        private float _experience;
        private float _coins;

        public static RewardProvider Instance { get; private set; }

        public RewardProvider(RewardService rewardService,
            LeaderBoardProvider leaderboard,
            AccrualConfig accrualConfig,
            TimerProvider timerProvider)
        {
            _rewardService = rewardService;
            _leaderboard = leaderboard;
            _accrualConfig = accrualConfig;
            _timerProvider = timerProvider;
            Instance = this;
        }

        public void ApplyEndMatchRewards()
        {
            if (PhotonNetwork.Time - _timerProvider.LocalStartTime < _accrualConfig.MinMatchTime)
            {
                return;
            }

            int playersCount = _leaderboard.GetTable().Count;

            var exp = _leaderboard.MyPlace * _accrualConfig.ExpPerPlace[playersCount];
            _experience += exp;

            var coins = _leaderboard.MyPlace * _accrualConfig.CoinsPerPlace[playersCount];
            _coins += coins;

            ApplyCoins();
            ApplyExp();
        }

        public void ApplyKill()
        {
            int playersCount = _leaderboard.GetTable().Count;
            var exp = _accrualConfig.ExpPerKill[playersCount];
            _experience += exp;

            var coins = _accrualConfig.CoinsPerKill[playersCount];
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

            var coinsReward = GameObject.Instantiate(_accrualConfig.RewardConfig);
            coinsReward.Count = (int) _coins;
            _coins -= (int) _coins;

            _rewardService.AddReward(coinsReward);
        }
    }
}