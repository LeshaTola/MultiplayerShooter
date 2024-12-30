using System;
using System.Collections.Generic;
using __Project.Scripts.Helpers;
using App.Scripts.Features.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens
{
    public class TopView : GameScreen
    {
        public event Action<int> OnToggleClicked; 
        public event Action OnSettingsClicked;
        
        [SerializeField] private List<ToggleCustom> _toggles;
        [SerializeField] private Button _settingsButton;

        public override void Initialize()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnValueChanged.AddListener(OnToggleChanged);
            }
            
            _settingsButton.onClick.AddListener(OnSettingsClick);
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