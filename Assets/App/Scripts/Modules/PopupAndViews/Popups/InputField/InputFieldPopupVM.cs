using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds.Providers;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InputFieldPopupVM
    {
        public InputFieldPopupData Data { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ISoundProvider SoundProvider { get; set; }

        public InputFieldPopupVM(
            ILocalizationSystem localizationSystem,
            InputFieldPopupData data,
            ISoundProvider soundProvider)
        {
            Data = data;
            SoundProvider = soundProvider;
            LocalizationSystem = localizationSystem;
        }
    }
}