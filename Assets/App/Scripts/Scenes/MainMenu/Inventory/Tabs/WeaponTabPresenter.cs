using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class WeaponTabPresenter : InventoryTabPresenter
    {
        public WeaponTabPresenter(InventoryTab view, IFactory<Item> itemFactory, IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider, RectTransform overlayTransform) : base(view, itemFactory, slotFactory,
            inventoryProvider, overlayTransform)
        {
        }

        public override void Initialize()
        {
            foreach (var weapon in InventoryProvider.GlobalInventory.Weapons)
            {
                var slot = SlotFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1);
                var item = ItemFactory.GetItem();
                item.Initialize(OverlayTransform, weapon.Sprite, weapon.Id);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
            }
        }
    }
}