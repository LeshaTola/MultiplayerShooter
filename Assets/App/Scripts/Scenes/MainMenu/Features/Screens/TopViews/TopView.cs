using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.CustomToggles;
using App.Scripts.Modules.Sounds;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    public class TopView : GameScreen
    {
        public event Action<int> OnToggleClicked;
        
        [SerializeField] private AudioDatabase _audioDatabase;
        [SerializeField] private List<ToggleCustom> _toggles;

        public override void Initialize()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnValueChanged.AddListener(OnToggleChanged);
            }
        }

        public void SetTab(int index)
        {
            _toggles[index].IsOn = true;
        }

        private void OnToggleChanged(bool isActive, int index)
        {
            if (!isActive)
            {
                return;
            }

            OnToggleClicked?.Invoke(index);
        }
    }
}