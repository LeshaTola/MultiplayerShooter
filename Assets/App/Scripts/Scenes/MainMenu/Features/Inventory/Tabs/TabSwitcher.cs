using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.CustomToggles;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs
{
    public class TabSwitcher : MonoBehaviour
    {
        [SerializeField] List<GameScreen> _tabs = new();
        [SerializeField] List<ToggleCustom> _toggles = new();

        private int _tabIndex;
        private List<InventoryTabPresenter> _inventoryTabPresenters;

        [Inject]
        public void Construct(List<InventoryTabPresenter> inventoryTabPresenters)
        {
            _inventoryTabPresenters = inventoryTabPresenters;
        }
        
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

            _tabIndex = 0;
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
                _inventoryTabPresenters[index].Hide();
                return;
            }
            _inventoryTabPresenters[index].Show();
            _tabIndex = index;
        }

        public void Show()
        {
            _inventoryTabPresenters[_tabIndex].Show();
        }

        public void Hide()
        {
            _inventoryTabPresenters[_tabIndex].Hide();
        }
    }
}