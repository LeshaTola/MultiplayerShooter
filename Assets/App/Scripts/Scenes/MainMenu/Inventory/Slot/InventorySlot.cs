using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Scenes.MainMenu.Inventory.Slot
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        private IInventorySlotStrategy _inventorySlotStrategy;
        public int SlotIndex { get; private set; }
        public Item Item { get; set; } = null;

        public void Initialize(IInventorySlotStrategy inventorySlotStrategy,
            int slotIndex)
        {
            _inventorySlotStrategy = inventorySlotStrategy;
            SlotIndex = slotIndex;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<Item>();
            _inventorySlotStrategy.Drop(this,item);
        }
    }
}