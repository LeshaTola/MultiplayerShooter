using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.CustomToggles;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    public class TopView : GameScreen
    {
        public event Action<int> OnToggleClicked; 
        public event Action OnSettingsClicked;
        public event Action OnCloseClicked;
        
        [SerializeField] private List<ToggleCustom> _toggles;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _closeButton;

        public override void Initialize()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnValueChanged.AddListener(OnToggleChanged);
            }
            
            _settingsButton.onClick.AddListener(OnSettingsClick);
            _closeButton.onClick.AddListener(GoToMainMenu);
        }

        public void SetLastToggle()
        {
            _toggles.Last().IsOn = true;
        }

        private void GoToMainMenu()
        {
            OnCloseClicked?.Invoke();
        }

        private void OnSettingsClick()
        {
            OnSettingsClicked?.Invoke();
        }

        private void OnToggleChanged(bool isActive, int index)
        {
            OnToggleClicked?.Invoke(index);
        }
    }
}