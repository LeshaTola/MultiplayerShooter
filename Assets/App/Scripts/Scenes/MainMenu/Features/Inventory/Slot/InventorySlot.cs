using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Slot
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _selectionImage;
        [SerializeField] private Image _rarityImage;
        [SerializeField] private TextMeshProUGUI _keyText;
        [SerializeField] private bool isMobile;
        
        private IInventorySlotStrategy _inventorySlotStrategy;
        public ItemType Type { get; private set; }
        public int SlotIndex { get; private set; }
        public Item Item { get; set; }

        public void Initialize(IInventorySlotStrategy inventorySlotStrategy,
            int slotIndex, Color rarityColor ,string keyText = "" ,ItemType itemType = ItemType.Weapon)
        {
            Type = itemType;
            _inventorySlotStrategy = inventorySlotStrategy;
            SlotIndex = slotIndex;
            _keyText.text = keyText;
            _rarityImage.color = rarityColor;
        }

        public void Select()
        {
            _selectionImage.gameObject.SetActive(true);
        }

        public void Unselect()
        {
            _selectionImage.gameObject.SetActive(false);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (isMobile)
            {
                return;
            }
            
            var item = eventData.pointerDrag.GetComponent<Item>();
            _inventorySlotStrategy.Drop(this,item);
        }
    }
}