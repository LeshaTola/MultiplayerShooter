namespace App.Scripts.Modules.Localization.Localizers
{
    public interface ITextLocalizer
    {
        public void Initialize(ILocalizationSystem localizationSystem);
        public void Translate();
    }
}