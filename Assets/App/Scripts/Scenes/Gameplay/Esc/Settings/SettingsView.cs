using System;
using App.Scripts.Features.Settings;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Esc.Settings
{
    public class SettingsView : MonoBehaviour
    {
        public event Action OnCloseButtonClicked;
        
        [SerializeField] private  Button _closeButton;
        [Space]
        [SerializeField] private Slider _mouseSensitivitySlider;
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private SettingsProvider _settingsProvider;

        public void Initialize(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            var audioService = settingsProvider.AudioService;
            var mouseSensitivityProvider = settingsProvider.SensivityProvider;
            
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            
            _mouseSensitivitySlider.value = mouseSensitivityProvider.SensivityNormalized;
            _mouseSensitivitySlider.onValueChanged.AddListener(value =>
            {
                mouseSensitivityProvider.SensivityNormalized = value;
            });
            
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

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _settingsProvider.SaveState();
        }
    }
}