using App.Scripts.Features;
using App.Scripts.Features.Commands;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Popups.Image
{
    public class ImagePopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;

        public ImagePopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
        }
        
        private ImagePopup _popup;

        public async UniTask ShowPopup(ImagePopupData popupData)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<ImagePopup>();
            }

            var viewModule = new ImagePopupVM(_localizationSystem, popupData);
            _popup.Setup(viewModule);

            await _popup.Show();
        }
        
        public async UniTask ShowPopup(string header, Sprite sprite)
        {
            await ShowPopup(new ImagePopupData()
            {
                Header = header,
                Image = sprite,
                Command = new CustomCommand(ConstStrings.CONFIRM, async () =>
                {
                    await HidePopup();
                })
            });
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