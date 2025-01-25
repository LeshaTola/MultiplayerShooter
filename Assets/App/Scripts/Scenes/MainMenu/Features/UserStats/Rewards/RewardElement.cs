using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.Views;
using App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardElement : AnimatedView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMPLocalizer _nameText;
        [SerializeField] private TextMeshProUGUI _countText;

        private ILocalizationSystem _localizationSystem;
        
        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _nameText.Initialize(_localizationSystem);
        }
        
        public void Setup(RewardConfig config)
        {
            _image.sprite = config.Reward.Sprite;
            _nameText.Key = config.Reward.Id;
            _nameText.Translate();
            _countText.text = config.Count.ToString();
        }

        public void Cleanup()
        {
            _nameText.Cleanup();
        }
    }
}