using System;
using App.Scripts.Features;
using App.Scripts.Features.Commands;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.Sounds.Providers;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InputFieldPopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly ISoundProvider _soundProvider;

        public InputFieldPopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem, ISoundProvider soundProvider)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
            _soundProvider = soundProvider;
        }
        
        private InputFieldPopup _popup;

        public async UniTask<string> ShowPopup(InputFieldPopupData popupData)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<InputFieldPopup>();
            }

            var viewModule = new InputFieldPopupVM(_localizationSystem, popupData, _soundProvider);
            _popup.Setup(viewModule);

            await _popup.Show();
            
            return await _popup.WaitResults();
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