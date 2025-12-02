using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InfoPopup : Popup
    {
        [ValueDropdown(@"GetAudioKeys")] [SerializeField] private string _closeSound;
        
        [SerializeField] private TMPLocalizer _header;
        [SerializeField] private TMPLocalizer _info;
        [SerializeField] private TMPLocalizedButton _okButton;

        private InfoPopupVM _vm;

        public void Setup(InfoPopupVM vm)
        {
            _vm = vm;

            Initialize();
            LocalSetup();
            Translate();
        }
        
        public override async UniTask Hide()
        {
            await base.Hide();
            Cleanup();
        }

        private void Cleanup()
        {
            _header.Cleanup();
            _info.Cleanup();
            _okButton.Cleanup();
        }

        private void LocalSetup()
        {
            _header.Key = _vm.Data.Header;
            _info.Key = _vm.Data.Mesage;
            _okButton.UpdateText(_vm.Data.Command.Label);
            _okButton.UpdateAction(() =>
            {
                _vm.SoundProvider.PlayOneShotSound(_closeSound);
                _vm.Data.Command.Execute();
            });
        }

        private void Initialize()
        {
            _header.Initialize(_vm.LocalizationSystem);
            _info.Initialize(_vm.LocalizationSystem);
            _okButton.Initialize(_vm.LocalizationSystem);
        }

        private void Translate()
        {
            _header.Translate();
            _info.Translate();
            _okButton.Translate();
        }
    }
}