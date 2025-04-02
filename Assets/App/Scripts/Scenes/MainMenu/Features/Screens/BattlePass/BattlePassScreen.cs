using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Features.Screens;
using App.Scripts.Features.UserStats.Rank;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.Sounds;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass
{
    public class BattlePassScreen : GameScreen
    {
        public event Action<int> OnRewardSelected;

        [Header("Rank Info")]
        [SerializeField] private TMPLocalizer _rankValue;
        [SerializeField] private TextMeshProUGUI _curExpValue;
        [SerializeField] private TextMeshProUGUI _maxExpValue;

        [Header("Reward Info")]
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMPLocalizer _rewardName;
        [SerializeField] private Button _rewardButton;

        [Header("Rewards")]
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ClickSond { get; private set; }
        [SerializeField] private BattlePassElement _battlePassElementPrefab;
        [SerializeField] private RectTransform _rewardsContainer;

        private readonly List<BattlePassElement> _elements = new();

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _rankValue.Initialize(localizationSystem);
            _rewardName.Initialize(localizationSystem);
        }

        public void UpdateSlider(UserRankProvider userRankProvider)
        {
            int id = 0;
            foreach (var element in _elements)
            {
                float sliderValue = 0;
                if (userRankProvider.CurrentRankId > id)
                {
                    sliderValue = 1f;
                }
                else if (userRankProvider.CurrentRankId == id)
                {
                    sliderValue = (float) userRankProvider.Experience / userRankProvider.CurrentRank.ExpForRank;
                }

                element.UpdateSlider(sliderValue);
                id++;
            }
        }

        public override void Cleanup()
        {
            CleanupRewards();
            _rankValue.Cleanup();
            _rewardName.Cleanup();
        }

        public void SetupRankInfo(RankData rankData, int curExp)
        {
            _rankValue.Key = rankData.Name;
            _rankValue.Text.color = rankData.RankColor;
            _rankValue.Translate();

            _curExpValue.text = curExp.ToString();
            _maxExpValue.text = rankData.ExpForRank.ToString();
        }

        public void SetupRewardInfo(RewardConfig rewardConfig)
        {
            if (rewardConfig == default)
            {
                _rewardImage.gameObject.SetActive(false);
                _rewardName.Key = "";
                _rewardName.Text.text = "";
                return;
            }

            _rewardImage.gameObject.SetActive(true);
            _rewardImage.sprite = rewardConfig.Reward.Sprite;
            _rewardName.Key = rewardConfig.Reward.Id;
            _rewardName.Translate();
        }

        public void SetupRewards(UserRankProvider userRankProvider)
        {
            CleanupRewards();
            var id = 0;
            foreach (var rank in userRankProvider.RanksDatabase.Ranks)
            {
                var newElement = Instantiate(_battlePassElementPrefab, _rewardsContainer);
                newElement.Initialize();
                var rewardSprite = rank.Rewards.Count > 0 ? rank.Rewards[0].Reward.Sprite : null;

                newElement.Setup(id, rewardSprite, rank.Sprite);
                newElement.OnButtonClicked += SelectReward;
                _elements.Add(newElement);

                id++;
            }

            UpdateSlider(userRankProvider);
        }

        private void CleanupRewards()
        {
            foreach (var element in _elements)
            {
                element.OnButtonClicked -= SelectReward;
                element.Cleanup();
                Destroy(element.gameObject);
            }

            _elements.Clear();
        }

        private void SelectReward(int id)
        {
            OnRewardSelected?.Invoke(id);
        }
        
        
        public List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }
    }
}