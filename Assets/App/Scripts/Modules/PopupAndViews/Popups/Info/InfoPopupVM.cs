using App.Scripts.Modules.Localization;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InfoPopupVM
    {
        public InfoPopupData Data { get; }
        public ILocalizationSystem LocalizationSystem { get; }

        public InfoPopupVM(ILocalizationSystem localizationSystem, InfoPopupData data)
        {
            Data = data;
            LocalizationSystem = localizationSystem;
        }
    }
}