using System.Threading;
using App.Scripts.Features;
using App.Scripts.Modules.Localization.Elements.Buttons;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.General.Popup;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Modules.PopupAndViews.Popups.Info
{
    public class InputFieldPopup : Popup
    {
        [ValueDropdown(@"GetAudioKeys")] [SerializeField] private string _closeSound;
        
        [SerializeField] private TMPLocalizer _header;
        [SerializeField] private TMPLocalizer _info;
        [SerializeField] private TMPLocalizer _placeholder;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMPLocalizedButton _okButton;

        private InputFieldPopupVM _vm;
        private CancellationTokenSource _cts;
        private UniTaskCompletionSource<string> _completionSource;

        public void Setup(InputFieldPopupVM vm)
        {
            _vm = vm;

            _completionSource = new UniTaskCompletionSource<string>();
            Initialize();
            LocalSetup();
            Translate();
        }

        public async UniTask<string> WaitResults()
        {
            return await _completionSource.Task;
        }
        
        public override async UniTask Hide()
        {
            _completionSource?.TrySetResult(null);
            Cleanup();
            await base.Hide();
        }

        private void LocalSetup()
        {
            _header.Key = _vm.Data.Header;
            _info.Key = _vm.Data.Mesage;
            _placeholder.Key = _vm.Data.Placeholder;
            _inputField.text = _vm.Data.StartValue;
            
            _okButton.UpdateText(_vm.Data.Command.Label);
            _okButton.UpdateAction(() =>
            {
                _vm.SoundProvider.PlaySound(_closeSound);
                _completionSource.TrySetResult(_inputField.text);
                _vm.Data.Command.Execute();
            });
        }

        private void Initialize()
        {
            _header.Initialize(_vm.LocalizationSystem);
            _info.Initialize(_vm.LocalizationSystem);
            _placeholder.Initialize(_vm.LocalizationSystem);
            _okButton.Initialize(_vm.LocalizationSystem);
            
            _inputField.onValueChanged.AddListener(value=>_vm.Data.OnValueChanged?.Invoke(value));
            _inputField.onEndEdit.AddListener(value=>_vm.Data.OnEndEdit?.Invoke(value));
        }

        private void Cleanup()
        {
            _header.Cleanup();
            _info.Cleanup();
            _placeholder.Cleanup();
            _okButton.Cleanup();
            
            _inputField.onEndEdit.RemoveAllListeners();
            _inputField.onValueChanged.RemoveAllListeners();
            
            _completionSource = null;
        }

        private void Translate()
        {
            _header.Translate();
            _info.Translate();
            _placeholder.Translate();
            _okButton.Translate();
        }
    }
}