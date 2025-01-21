using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Settings;
using App.Scripts.Modules.CustomToggles;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Esc.Settings
{
    public class SettingsView : MonoBehaviour
    {
        public event Action OnCloseButtonClicked;
        
        [SerializeField] private  Button _closeButton;
        [Space]
        [SerializeField] private Button _localizationButton;
        [SerializeField] private TextMeshProUGUI _localizationText;
        [SerializeField] private Image _localizationImage;
        [Space]
        [SerializeField] private SliderCover _mouseSensitivitySlider;
        [Space]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private SettingsProvider _settingsProvider;
        private List<string> _languages;
        private int _languageIndex;

        public void Initialize(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            var audioService = settingsProvider.AudioService;
            var mouseSensitivityProvider = settingsProvider.SensivityProvider;
            var localizationSystem = settingsProvider.LocalizationSystem;
            
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            
            _localizationButton.onClick.AddListener(SwapLanguage);
            _languages = localizationSystem.GetLanguages().Select(x=>x.Key).ToList();
            _languageIndex = _languages.FindIndex(x=>x.Equals(localizationSystem.Language));
            SetLanguage(localizationSystem.Language);

            InitializeMouseSensivity(mouseSensitivityProvider);
            InitializeAudioSettings(audioService);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _settingsProvider.SaveState();
        }

        private void SetLanguage( string curLanguage)
        {
            var  localizationSystem = _settingsProvider.LocalizationSystem;
            _localizationImage.sprite = localizationSystem.GetLanguages()[curLanguage].Sprite;
            _localizationText.text = localizationSystem.GetLanguages()[curLanguage].Name;
        }

        private void SwapLanguage()
        {
            _languageIndex = (_languageIndex + 1) % _languages.Count;
            var language = _languages[_languageIndex];
            SetLanguage(language);
            _settingsProvider.LocalizationSystem.ChangeLanguage(language);
        }

        private void InitializeMouseSensivity(MouseSensivityProvider mouseSensitivityProvider)
        {
            _mouseSensitivitySlider.Slider.onValueChanged.AddListener(OnSensivitySliderChanged);
            _mouseSensitivitySlider.Slider.value = mouseSensitivityProvider.SensivityNormalized;
        }

        private void InitializeAudioSettings(IAudioService audioService)
        {
            _masterVolumeSlider.value = audioService.MasterVolume;
            _masterVolumeSlider.onValueChanged.AddListener(value =>
            {
                audioService.MasterVolume = value;
            });

            _musicVolumeSlider.value = audioService.MusicVolume;
            _musicVolumeSlider.onValueChanged.AddListener(value =>
            {
                audioService.MusicVolume= value;
            });

            _sfxVolumeSlider.value = audioService.EffectsVolume;
            _sfxVolumeSlider.onValueChanged.AddListener(value =>
            {
                audioService.EffectsVolume= value;
            });
        }
        
        private void OnSensivitySliderChanged(float value)
        {
            _settingsProvider.SensivityProvider.SensivityNormalized = value;
            _mouseSensitivitySlider.ValueText.text = _settingsProvider.SensivityProvider.Sensivity.ToString("0.00");
        }
    }
}