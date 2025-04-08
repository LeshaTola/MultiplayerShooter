using System;
using App.Scripts.Modules.PopupAndViews.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class InputPasswordView : AnimatedView
    {
        // [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private InputField _passwordInput;

        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _closeButton;
        
        private Action<string> _confirmAction;

        public void Setup(Action<string> confirmAction)
        {
            _confirmAction = confirmAction;
        }
        
        private void OnEnable()
        {
            _passwordInput.text = "";
            
            _passwordInput.onValueChanged.AddListener(ValidatePassword);
            _confirmButton.onClick.AddListener(()=> _confirmAction?.Invoke(_passwordInput.text));
            _closeButton.onClick.AddListener( async ()=> await Hide());
        }

        private void OnDisable()
        {
            _passwordInput.onValueChanged.RemoveAllListeners();
            _confirmButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }
        
        private void ValidatePassword(string input)
        {
            if (input.Length > CreateRoomView.MAX_PASSWORD_LENGTH)
            {
                _passwordInput.text = input.Substring(0, CreateRoomView.MAX_PASSWORD_LENGTH);
            }
        }
    }
}