using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds.Providers;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InfoPopupVM
    {
        public InfoPopupData Data { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ISoundProvider SoundProvider { get; set; }

        public InfoPopupVM(ILocalizationSystem localizationSystem, InfoPopupData data, ISoundProvider soundProvider)
        {
            Data = data;
            SoundProvider = soundProvider;
            LocalizationSystem = localizationSystem;
        }
    }
}