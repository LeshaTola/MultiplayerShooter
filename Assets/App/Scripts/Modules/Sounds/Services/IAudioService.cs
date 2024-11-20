namespace App.Scripts.Modules.Sounds.Services
{
    public interface IAudioService
    {
        float MasterVolume { get; set; }
        float MusicVolume { get; set; }
        float EffectsVolume { get; set; }
    }
}