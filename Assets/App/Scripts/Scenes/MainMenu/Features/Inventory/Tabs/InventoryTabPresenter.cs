using App.Scripts.Features.Inventory;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs
{
    public class InventoryTabPresenter : GameScreenPresenter
    {
        protected readonly InventoryTab View;
        protected readonly IFactory<Item> ItemFactory;
        protected readonly IFactory<InventorySlot> SlotFactory;
        protected readonly InventoryProvider InventoryProvider;
        protected readonly RectTransform OverlayTransform;
        
        public InventoryTabPresenter(InventoryTab view,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider, RectTransform overlayTransform)
        {
            View = view;
            ItemFactory = itemFactory;
            SlotFactory = slotFactory;
            InventoryProvider = inventoryProvider;
            OverlayTransform = overlayTransform;
        }

        public override void Cleanup()
        {
            View.Cleanup();
        }
    }
}