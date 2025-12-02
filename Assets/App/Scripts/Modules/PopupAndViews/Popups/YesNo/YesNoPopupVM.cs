using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;

namespace App.Scripts.Modules.PopupAndViews.Popups.YesNo
{
    public class YesNoPopupVM
    {
        public YesNoPopupData Data { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ISoundProvider SoundProvider { get; set; }

        public YesNoPopupVM(ILocalizationSystem localizationSystem, YesNoPopupData data, ISoundProvider soundProvider)
        {
            Data = data;
            SoundProvider = soundProvider;
            LocalizationSystem = localizationSystem;
        }
    }
}