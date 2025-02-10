using App.Scripts.Features.Settings;

namespace App.Scripts.Features.Settings
{
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