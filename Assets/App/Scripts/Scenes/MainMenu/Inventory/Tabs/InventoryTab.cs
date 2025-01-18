using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class InventoryTab : GameScreen
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private List<GameObject> _tabElemets;

        public override UniTask Show()
        {
            foreach (var tabElemet in _tabElemets)
            {
                tabElemet.SetActive(true);
            }
            return base.Show();
        }

        public override UniTask Hide()
        {
            foreach (var tabElemet in _tabElemets)
            {
                tabElemet.SetActive(false);
            }
            return base.Hide();
        }

        public void AddSlot(InventorySlot slot)
        {
            slot.transform.SetParent(_container, false);
        }
        
        public override void Cleanup()
        {
            foreach (Transform child in _container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}