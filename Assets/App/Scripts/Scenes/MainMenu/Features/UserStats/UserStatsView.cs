using System;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization.Localizers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats
{
    public class UserStatsView : GameScreen
    {
        public event Action<string> OnPlayerNameChanged; 
        
        [SerializeField] private ImageLoadYG _playerImage;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TMP_InputField _playerInputField;

        [Header("Rank")]
        [SerializeField] private TMPLocalizer _rankName;
        [SerializeField] private Image _rankImage;
        [SerializeField] private Slider _rankSlider;
        [SerializeField] private TextMeshProUGUI _rankExpText;
        
        [Header("Tickets")]
        [SerializeField] private TextMeshProUGUI _ticketsText;
        
        public override void Initialize()
        {
            if (YG2.player.photo != null)
            {
                _playerImage.spriteImage.color = Color.white;
                _playerImage.Load(YG2.player.photo);
            }
            
            _playerInputField.onEndEdit.AddListener((value) =>
            {
                OnPlayerNameChanged?.Invoke(value);
            });
        }

        public void SetupName(string playerName)
        {
            _playerInputField.text = playerName;
        }

        public void SetupMoney(int money)
        {
            _moneyText.text = money.ToString();
        }
        
        public void SetupRank(string rankName, Sprite rankSprite, float curentExp, float maxExp)
        {
            _rankName.Key = rankName;
            _rankName.Translate();
            _rankImage.sprite = rankSprite;
            var normalizedExp = curentExp / maxExp;
            _rankSlider.value = normalizedExp;
            _rankExpText.text = $"{curentExp}/{maxExp}";
        }

        public void SetupTickets(int tickets)
        {
            _ticketsText.text = tickets.ToString();
        }

        public override void Cleanup()
        {
            _playerInputField.onEndEdit.RemoveAllListeners();
        }
    }
}