using System;
using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopup: Popup
    {
        [SerializeField] private TMPLocalizer _header;
        
        [Header("Rewards")]
        [SerializeField] private RewardElement _rewardElementPrefab;
        [SerializeField] private RectTransform _rewardsContainer;

        [Header("EXP")]
        [SerializeField] private Image _curRankImage;
        [SerializeField] private Image _nextRankImage;
        [SerializeField] private Slider _expSlider;
        [SerializeField] private float _expAnimationDuration = 0.3f;
        
        [Space]
        [SerializeField] private TMPLocalizedButton _acceptButton;
        [SerializeField] private float _buttonAnimationDuration = 0.3f;
        
        private RewardsPopupVM _vm;

        public void Setup(RewardsPopupVM vm)
        {
            _vm = vm;
            Initialize();
            _acceptButton.UpdateAction(()=>Hide().Forget());
            
            SetUpExpSlider();
            SetupRanks(_vm.RankProvider.CurrentRankId);

            Translate();
        }

        private void SetUpExpSlider()
        {
            _expSlider.value = (float)_vm.RankProvider.Experience / _vm.RankProvider.CurrentRank.ExpForRank;
        }

        public override async UniTask Show()
        {
            _acceptButton.transform.localScale = Vector3.zero;
            await base.Show();
            await ExpSliderAnimation();
            await SpawnRewards();
            await _acceptButton.transform.DOScale(Vector3.one, _buttonAnimationDuration).SetEase(Ease.OutBounce);
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            Cleanup();
        }
        
        private void Initialize()
        {
            _acceptButton.Initialize(_vm.LocalizationSystem);
            _header.Initialize(_vm.LocalizationSystem);
        }

        private void Translate()
        {
            _acceptButton.Translate();
            _header.Translate();
        }

        private void Cleanup()
        {
            _acceptButton.Cleanup();
            _header.Cleanup();
            foreach (Transform child in _rewardsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private async UniTask SpawnRewards()
        {
            foreach (var reward in _vm.Rewards)
            {
                var element = Instantiate(_rewardElementPrefab, _rewardsContainer);
                element.Initialize(_vm.LocalizationSystem);
                element.Setup(reward);
                await element.Show();
            }
        }

        private async UniTask ExpSliderAnimation()
        {
            var levelUps = _vm.LevelUps;
            
            while (levelUps > 0)
            {
                await DOVirtual.Float(_expSlider.value, _expSlider.maxValue, _expAnimationDuration, (value) =>
                {
                    _expSlider.value = value;
                });
                levelUps--;
                
                await LevelUp(_vm.RankProvider.CurrentRankId - levelUps);
                _expSlider.value = 0;
            }
            
            await DOVirtual.Float(_expSlider.value, _vm.ExpValue, _expAnimationDuration, (value) =>
            {
                _expSlider.value = value;
            });
        }

        private async UniTask LevelUp(int rankId)
        {
            SetupRanks(rankId);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        }
        
        private void SetupRanks(int rankId)
        {
            var curRank = _vm.RankProvider.RanksDatabase.Ranks[rankId];
            var nextRank = _vm.RankProvider.RanksDatabase.Ranks[rankId+1];
            SetupRanks(curRank.Sprite, nextRank.Sprite);
        }

        private void SetupRanks(Sprite cur, Sprite next)
        {
            _curRankImage.sprite = cur;
            _nextRankImage.sprite = next;
        }
    }
}