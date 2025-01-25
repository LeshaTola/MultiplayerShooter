using System;
using App.Scripts.Features.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats
{
    public class UserStatsView : GameScreen
    {
        public event Action<string> OnPlayerNameChanged; 
        
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TMP_InputField _playerInputField;

        [Header("Rank")]
        [SerializeField] private TextMeshProUGUI _rankName;
        [SerializeField] private Image _rankImage;
        [SerializeField] private Slider _rankSlider;
        
        public override void Initialize()
        {
            _playerInputField.onEndEdit.AddListener((value) =>
            {
                OnPlayerNameChanged?.Invoke(value);
            });
        }

        public void Setup(string playerName)
        {
            _playerInputField.text = playerName;
        }

        public void SetupMoney(int money)
        {
            _moneyText.text = money.ToString();
        }
        
        public void SetupRank(string rankName, Sprite rankSprite, float normalizedExp)
        {
            _rankName.text = rankName;
            _rankImage.sprite = rankSprite;
            _rankSlider.value = normalizedExp;
        }

        public override void Cleanup()
        {
            _playerInputField.onEndEdit.RemoveAllListeners();
        }
    }
}