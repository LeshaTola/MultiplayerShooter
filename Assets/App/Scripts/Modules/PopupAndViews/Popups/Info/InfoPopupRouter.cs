using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InfoPopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;

        public InfoPopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
        }
        
        private InfoPopup _popup;

        public async UniTask ShowPopup(InfoPopupData popupData)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<InfoPopup>();
            }

            var viewModule = new InfoPopupVM(_localizationSystem, popupData);
            _popup.Setup(viewModule);

            await _popup.Show();
        }
    }
}