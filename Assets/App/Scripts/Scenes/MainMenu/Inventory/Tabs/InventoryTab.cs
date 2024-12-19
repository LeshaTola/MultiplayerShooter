using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class InventoryTab : GameScreen
    {
        [SerializeField] private RectTransform _container;

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