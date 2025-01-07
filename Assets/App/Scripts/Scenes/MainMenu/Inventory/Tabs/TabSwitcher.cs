using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.CustomToggles;
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

        public void Cleanup()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnValueChanged.RemoveAllListeners();
            }
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