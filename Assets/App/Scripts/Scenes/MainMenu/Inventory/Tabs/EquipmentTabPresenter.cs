using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class EquipmentTabPresenter : InventoryTabPresenter
    {
        public EquipmentTabPresenter(InventoryTab view, IFactory<Item> itemFactory, IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider, RectTransform overlayTransform) : base(view, itemFactory, slotFactory,
            inventoryProvider, overlayTransform)
        {
        }

        public override void Initialize()
        {
            foreach (var quipmentId in InventoryProvider.Inventory.Equipment)
            {
                var slot = SlotFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1, "",ItemType.Equipment);
                var item = ItemFactory.GetItem();
                var equipmentConfig = InventoryProvider.EquipmentById(quipmentId);
                item.Initialize(null, OverlayTransform, equipmentConfig.Sprite, quipmentId, ItemType.Equipment);//TODO null isnt correct;
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
            }
        }
    }
}