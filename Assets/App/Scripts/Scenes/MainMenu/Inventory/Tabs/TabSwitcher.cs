﻿using System.Collections.Generic;
using __Project.Scripts.Helpers;
using App.Scripts.Features.Screens;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class TabSwitcher : MonoBehaviour
    {
        [SerializeField] List<GameScreen> _tabs = new();
        [SerializeField] List<ToggleCustom> _toggles = new();

        public void Initialize()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnValueChanged.AddListener(ChangeTab);
            }
            
            foreach (var tab in _tabs)
            {
                tab.Hide();
            }

            _tabs[0].Show();
        }

        private void ChangeTab(bool value, int index)
        {
            if (!value)
            {
                _tabs[index].Hide();
                return;
            }
            _tabs[index].Show();
        }
    }
}