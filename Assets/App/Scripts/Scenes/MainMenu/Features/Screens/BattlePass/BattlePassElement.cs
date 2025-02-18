using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass
{
    public class BattlePassElement : MonoBehaviour
    {
        public event Action<int> OnButtonClicked;
        
        [SerializeField] private Image _rewardImage;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Image _rankImage;
        [SerializeField] private Image _progressImage;
        [SerializeField] private Slider _expSlider;

        [Space]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _completeColor = Color.yellow;
        
        private int _rankId;

        public void Initialize()
        {
            _rewardButton.onClick.AddListener(() => OnButtonClicked?.Invoke(_rankId));
        }

        public void Cleanup()
        {
            _rewardButton.onClick.RemoveAllListeners();
        }
        
        public void Setup(int rankId ,Sprite rewardSprite, Sprite rankSprite)
        {
            _rankId = rankId;
            if (rewardSprite == null)
            {
                _rewardImage.gameObject.SetActive(false);    
            }
            _rewardImage.sprite = rewardSprite;
            _rankImage.sprite = rankSprite;
        }

        public void UpdateSlider(float sliderValue)
        {
            _expSlider.value = sliderValue;
            _progressImage.color = sliderValue > 0 ? _completeColor : _defaultColor;
        }
    }
}