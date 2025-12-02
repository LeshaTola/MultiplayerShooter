using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using App.Scripts.Modules.Sounds;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopup: Popup
    {
        [ValueDropdown(@"GetAudioKeys")] [SerializeField] private string _awardSound;
        [ValueDropdown(@"GetAudioKeys")] [SerializeField] private string _closeSound;

        [SerializeField] private TMPLocalizer _header;
        
        [Header("Rewards")]
        [SerializeField] private RewardElement _rewardElementPrefab;
        [SerializeField] private RectTransform _rewardsContainer;

        [Header("EXP")]
        [SerializeField] private Image _curRankImage;
        [SerializeField] private Image _nextRankImage;
        [SerializeField] private Slider _expSlider;
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private float _expAnimationDuration = 0.3f;
        
        [Space]
        [SerializeField] private TMPLocalizedButton _acceptButton;
        [SerializeField] private float _buttonAnimationDuration = 0.3f;
        
        private RewardsPopupVM _vm;

        public void Setup(RewardsPopupVM vm)
        {
            _vm = vm;
            Initialize();
            _acceptButton.UpdateAction(Close);
            
            SetUpExp();
            Translate();
        }

        private void SetUpExp()
        {
            var animationData = _vm.AnimationDatas[0];
            SetupRanks(animationData.FromSprite, animationData.ToSprite);

            _expSlider.value = animationData.FromExp / animationData.MaxExp;
            _expText.text = animationData.FromExp  + "/" + animationData.MaxExp;
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
                _vm.SoundProvider.PlayOneShotSound(_awardSound);
                var element = Instantiate(_rewardElementPrefab, _rewardsContainer);
                element.Initialize(_vm.LocalizationSystem);
                element.Setup(reward);
                await element.Show();
            }
        }

        private async UniTask ExpSliderAnimation()
        {
            if (_vm.AnimationDatas[0].FromExp.Equals(_vm.AnimationDatas[0].ToExp))
            {
                return;
            }
            
            foreach (var animationData in _vm.AnimationDatas)
            {
                SetupRanks(animationData.FromSprite, animationData.ToSprite);
                await DOVirtual.Float(animationData.FromExp, animationData.ToExp, _expAnimationDuration, (value) =>
                {
                    _expSlider.value = value/animationData.MaxExp;
                    _expText.text = (int)value + "/" + animationData.MaxExp;
                });
            }
        }

        private void SetupRanks(Sprite cur, Sprite next)
        {
            _curRankImage.sprite = cur;
            _nextRankImage.sprite = next;
        }
        
        private void Close()
        {
            _vm.SoundProvider.PlayOneShotSound(_closeSound);
            Hide().Forget();
        }
    }
}