using System;
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

        private IAudioService _audioService;
        private MouseSensivityProvider _mouseSensitivityProvider;

        public void Initialize(IAudioService audioService, MouseSensivityProvider mouseSensitivityProvider)
        {
            _audioService = audioService;
            _mouseSensitivityProvider = mouseSensitivityProvider;
            
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            
            _mouseSensitivitySlider.value = _mouseSensitivityProvider.SensivityNormalized;
            _mouseSensitivitySlider.onValueChanged.AddListener(value =>
            {
                _mouseSensitivityProvider.SensivityNormalized = value;
                PlayerPrefs.SetFloat(MouseSensivityProvider.SETTINGS_SAVES, _mouseSensitivityProvider.Sensivity);
            });
            
            _masterVolumeSlider.value = _audioService.MasterVolume;
            _masterVolumeSlider.onValueChanged.AddListener(value =>
            {
                _audioService.MasterVolume = value;
            });

            _musicVolumeSlider.value = _audioService.MusicVolume;
            _musicVolumeSlider.onValueChanged.AddListener(value =>
            {
                _audioService.MusicVolume= value;
            });

            _sfxVolumeSlider.value = _audioService.EffectsVolume;
            _sfxVolumeSlider.onValueChanged.AddListener(value =>
            {
                _audioService.EffectsVolume= value;
            });
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}