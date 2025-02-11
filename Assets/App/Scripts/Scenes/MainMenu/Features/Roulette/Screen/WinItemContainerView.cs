using App.Scripts.Features.Rewards.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Screen
{
    public class WinItemContainerView : MonoBehaviour
    {
        [SerializeField] private Image _rareImage;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _countText;

        public void Setup(Color rareColor, RewardConfig rewardConfig)
        {
            _rareImage.color = rareColor;
            _itemImage.sprite = rewardConfig.Reward.Sprite;
            
            if (rewardConfig.Count <= 1)
            {
                _countText.gameObject.SetActive(false);
                return;
            }
            _countText.text = rewardConfig.Count.ToString();
        }
        
    }
}