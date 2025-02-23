using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Popups.Image
{
    public class ImagePopup : Popup
    {
        [SerializeField] private TMPLocalizer _header;
        [SerializeField] private UnityEngine.UI.Image _image;
        [SerializeField] private TMPLocalizedButton _okButton;
        
        private ImagePopupVM _vm;

        public void Setup(ImagePopupVM vm)
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
            _okButton.Cleanup();
        }

        private void LocalSetup()
        {
            _header.Key = _vm.Data.Header;
            _image.sprite = _vm.Data.Image;
            _okButton.UpdateText(_vm.Data.Command.Label);
            _okButton.UpdateAction(_vm.Data.Command.Execute);
        }

        private void Initialize()
        {
            _header.Initialize(_vm.LocalizationSystem);
            _okButton.Initialize(_vm.LocalizationSystem);
        }

        private void Translate()
        {
            _header.Translate();
            _okButton.Translate();
        }
    }
}