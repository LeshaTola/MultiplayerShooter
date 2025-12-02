using App.Scripts.Features;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.PopupAndViews.General.Providers;
using App.Scripts.Modules.Sounds.Providers;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.PopupAndViews.Popups.YesNo
{
    public class YesNoPopupRouter : IPopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly ISoundProvider _soundProvider;

        public YesNoPopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem, ISoundProvider soundProvider)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
            _soundProvider = soundProvider;
        }

        public async UniTask ShowPopup(string header, string message)
        {
            await ShowPopup(new YesNoPopupData()
            {
                Header = header,
                Message = message,
                YesCommand = new CustomCommand(ConstStrings.CONFIRM, async () => { await HidePopup(); }),
                NoCommand = new CustomCommand(ConstStrings.CONFIRM, async () => { await HidePopup(); })
            });
        }

        public async UniTask ShowPopup(YesNoPopupData popupData)
        {
            var popup = _popupController.GetPopup<YesNoPopup>();
            
            var viewModule = new YesNoPopupVM(_localizationSystem, popupData, _soundProvider);
            popup.Setup(viewModule);

            await popup.Show();
        }
        
        public async UniTask HidePopup()
        {
            await _popupController.HideLastPopup();
        }

        private async void Hide()
        {
            await HidePopup();
        }
    }
}