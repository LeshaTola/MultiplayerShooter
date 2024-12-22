using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller.Providers
{
    public class MouseSensivityProvider
    {
        public const string SETTINGS_SAVES = "SettingsSaves";

        private readonly MouseSensivityConfig _config;

        public MouseSensivityProvider(MouseSensivityConfig config)
        {
            _config = config;
            Sensivity = PlayerPrefs.GetFloat(SETTINGS_SAVES, _config.MouseSensivity);
        }

        public float Sensivity { get; private set; }
        
        public float SensivityNormalized
        {
            get => _config.MinMaxSensivity.InverseLerp(Sensivity);
            set
            {
                Sensivity = _config.MinMaxSensivity.Lerp(value);
            }
        }

    }
}