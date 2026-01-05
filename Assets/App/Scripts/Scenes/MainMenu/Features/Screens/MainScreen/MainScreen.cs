using System;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization.Localizers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen
{
    public class MainScreen : GameScreen
    {
        public event Action OnSettingsClicked;
        public event Action OnTutorClicked;
        
        public event Action PlayButtonAction;
        public event Action RouletteButtonAction;
        public event Action BattlePassButtonAction;
        public event Action RegionChangedAction;
        
        [Header("Main")]
        [SerializeField] private Button _playButton;
        
        [Header("Tools")]
        [SerializeField] private RegionDropdownHandler _regionDropdownHandler;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _tutorButton;

        [Header("Roulette")]
        [SerializeField] private Button _rouletteButton;
        
        [Header("Battle Pass")]
        [SerializeField] private Button _battlePassButton;
        [SerializeField] private Image _rankImage;
        [SerializeField] private TMPLocalizer _rankName;

        [SerializeField] private TextMeshProUGUI _ticketsCountText;
        [SerializeField] private TextMeshProUGUI _versionText;
        
        public override void Initialize()
        {
            _playButton.onClick.AddListener(() => PlayButtonAction?.Invoke());
            _rouletteButton.onClick.AddListener(() => RouletteButtonAction?.Invoke());
            _battlePassButton.onClick.AddListener(() => BattlePassButtonAction?.Invoke());
            
            _regionDropdownHandler.Initialize();
            
            _settingsButton.onClick.AddListener(OnSettingsClick);
            _tutorButton.onClick.AddListener(OpenTutor);
            
            _versionText.text = Application.version;
        }

        public override void Cleanup()
        {
            _playButton.onClick.RemoveAllListeners();
            _rouletteButton.onClick.RemoveAllListeners();
            _battlePassButton.onClick.RemoveAllListeners();
        }

        public void SetTicketsCount(int ticketsCount)
        {
            _ticketsCountText.text = ticketsCount.ToString();
        }

        public void SetupRank(Sprite rankSprite, string rankName)
        {
            _rankImage.sprite = rankSprite;
            _rankName.Key = rankName;
            _rankName.Translate();
        }
        
        private void SetupRegions()
        {
            
        }

        private void OpenTutor()
        {
            OnTutorClicked?.Invoke();
        }

        private void OnSettingsClick()
        {
            OnSettingsClicked?.Invoke();
        }
        
    }
}