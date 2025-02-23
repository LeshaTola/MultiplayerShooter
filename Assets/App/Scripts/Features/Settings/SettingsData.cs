using System;
using App.Scripts.Features.Settings;
using UnityEngine;

namespace App.Scripts.Features.Settings
{
    [Serializable]
    public class SettingsData
    {
        public float MasterVolume;
        public float MusicVolume;
        public float EffectsVolume;
        
        public float MouseSensitivity;
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public SettingsData SettingsData;
    }
}