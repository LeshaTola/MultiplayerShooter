using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Popups.YesNo
{
    public class YesNoPopup : Popup
    {
        [SerializeField] private TMPLocalizer _header;
        [SerializeField] private TMPLocalizer _info;
        [SerializeField] private TMPLocalizedButton _yesButton;
        [SerializeField] private TMPLocalizedButton _noButton;

        private YesNoPopupVM _vm;

        public void Setup(YesNoPopupVM vm)
        {
            _vm = vm;

            _vm.SoundProvider.PlayOneShotSound(AudioKeys.PopUp_sound);
            Initialize();
            LocalSetup();
            Translate();
        }

        public override async UniTask Hide()
        {
            // _vm.SoundProvider.PlayOneShotSound(AudioKeys.Close);
            await base.Hide();
            Cleanup();
        }

        private void Cleanup()
        {
            _header.Cleanup();
            _info.Cleanup();
            _yesButton.Cleanup();
            _noButton.Cleanup();
        }

        private void LocalSetup()
        {
            _header.Key = _vm.Data.Header;
            _info.Key = _vm.Data.Message;
            if (_vm.Data.YesCommand == null)
            {
                _yesButton.gameObject.SetActive(false);
            }
            else
            {
                _yesButton.gameObject.SetActive(true);
                _yesButton.UpdateText(_vm.Data.YesCommand.Label);
                _yesButton.UpdateAction(() =>
                {
                    _vm.Data.YesCommand.Execute();
                });
            }
            
            if (_vm.Data.NoCommand == null)
            {
                _noButton.gameObject.SetActive(false);
            }
            else
            {
                _noButton.gameObject.SetActive(true);
                _noButton.UpdateText(_vm.Data.NoCommand.Label);
                _noButton.UpdateAction(() => { _vm.Data.NoCommand.Execute(); });
            }
        }

        private void Initialize()
        {
            _header.Initialize(_vm.LocalizationSystem);
            _info.Initialize(_vm.LocalizationSystem);
            _yesButton.Initialize(_vm.LocalizationSystem);
            _noButton.Initialize(_vm.LocalizationSystem);
        }

        private void Translate()
        {
            _header.Translate();
            _info.Translate();
            _yesButton.Translate();
            _noButton.Translate();
        }
    }
}