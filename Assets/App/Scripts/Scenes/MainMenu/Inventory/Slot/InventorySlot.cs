using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Inventory.Slot
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Color _weaponColor = Color.white;
        [SerializeField] private Color _equipmentColor = Color.white;
        [SerializeField] private Image _image;
        
        private IInventorySlotStrategy _inventorySlotStrategy;
        public int SlotIndex { get; private set; }
        public Item Item { get; set; } = null;

        public void Initialize(IInventorySlotStrategy inventorySlotStrategy,
            int slotIndex, ItemType itemType = ItemType.Weapon)
        {
            _inventorySlotStrategy = inventorySlotStrategy;
            SlotIndex = slotIndex;
            _image.color = itemType == ItemType.Weapon ? _weaponColor : _equipmentColor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<Item>();
            _inventorySlotStrategy.Drop(this,item);
        }
    }
}