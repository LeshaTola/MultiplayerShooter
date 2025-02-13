namespace App.Scripts.Scenes.Gameplay.Controller.Providers
{
    public class MouseSensivityProvider
    {

        private readonly MouseSensivityConfig _config;

        public MouseSensivityProvider(MouseSensivityConfig config)
        {
            _config = config;
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