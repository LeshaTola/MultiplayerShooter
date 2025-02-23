using App.Scripts.Modules.Localization;

namespace App.Scripts.Modules.PopupAndViews.Popups.Image
{
    public class ImagePopupVM
    {
        public ImagePopupData Data { get; }
        public ILocalizationSystem LocalizationSystem { get; }

        public ImagePopupVM(ILocalizationSystem localizationSystem, ImagePopupData data)
        {
            Data = data;
            LocalizationSystem = localizationSystem;
        }
    }
}