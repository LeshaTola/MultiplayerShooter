﻿using App.Scripts.Modules.Saves;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Scenes.Gameplay.Controller.Providers;

namespace App.Scripts.Features.Settings
{
    public class SettingsProvider : ISavable
    {
        private readonly IDataProvider<SettingsData> _dataProvider;
        
        public MouseSensivityProvider SensivityProvider { get; }
        public IAudioService AudioService { get; }
        
        public SettingsProvider(MouseSensivityProvider mouseSensivityProvider,
            IAudioService audioService,
            IDataProvider<SettingsData> dataProvider)
        {
            SensivityProvider = mouseSensivityProvider;
            AudioService = audioService;
            _dataProvider = dataProvider;
        }

        public void SaveState()
        {
            _dataProvider.SaveData(GetState());
        }

        public void LoadState()
        {
            if (!_dataProvider.HasData())
            {
                _dataProvider.SaveData(new SettingsData()
                {
                    MasterVolume = 1,
                    EffectsVolume = 1,
                    MusicVolume = 1,
                    MouseSensitivity = 1,
                });
            }

            SetState(_dataProvider.GetData());
        }
        
        private SettingsData GetState()
        {
            return new SettingsData
            {
                MasterVolume = AudioService.MasterVolume,
                MusicVolume = AudioService.MusicVolume,
                EffectsVolume = AudioService.EffectsVolume,
                MouseSensitivity = SensivityProvider.SensivityNormalized
            };
        }

        private void SetState(SettingsData data)
        {
            AudioService.MasterVolume = data.MasterVolume;
            AudioService.MusicVolume = data.MusicVolume;
            AudioService.EffectsVolume = data.EffectsVolume;
            SensivityProvider.SensivityNormalized = data.MouseSensitivity;
        }
    }
}