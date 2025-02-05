using System;
using App.Scripts.Features.Commands;
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
        
        public async UniTask ShowPopup(string header, string message)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<InfoPopup>();
            }

            var viewModule = new InfoPopupVM(_localizationSystem, new InfoPopupData()
            {
                Header = header,
                Mesage = message,
                Command = new CustomCommand("Принять", async () =>
                {
                    await HidePopup();
                })
            });
            _popup.Setup(viewModule);

            await _popup.Show();
        }

        public async UniTask HidePopup()
        {
            if (_popup == null)
            {
                return;
            }

            await _popup.Hide();
            _popup = null;
        }

        private async void Hide()
        {
            await HidePopup();
        }
    }
}