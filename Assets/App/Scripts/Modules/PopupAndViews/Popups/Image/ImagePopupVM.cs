using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds.Providers;

namespace App.Scripts.Modules.PopupAndViews.Popups.Image
{
    public class ImagePopupVM
    {
        public ImagePopupData Data { get; }
        public ILocalizationSystem LocalizationSystem { get; }
        public ISoundProvider SoundProvider { get; }

        public ImagePopupVM(ILocalizationSystem localizationSystem, ImagePopupData data, ISoundProvider soundProvider)
        {
            SoundProvider = soundProvider;
            Data = data;
            LocalizationSystem = localizationSystem;
        }
    }
}