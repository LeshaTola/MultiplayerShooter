using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Slot
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Color _weaponColor = Color.white;
        [SerializeField] private Color _equipmentColor = Color.white;
        [SerializeField] private Color _skinColor = Color.white;
        [SerializeField] private Image _image;
        [SerializeField] private Image _selectionImage;
        [SerializeField] private TextMeshProUGUI _keyText;
        
        private IInventorySlotStrategy _inventorySlotStrategy;
        public ItemType Type { get; private set; }
        public int SlotIndex { get; private set; }
        public Item Item { get; set; }

        public void Initialize(IInventorySlotStrategy inventorySlotStrategy,
            int slotIndex,string keyText = "" ,ItemType itemType = ItemType.Weapon)
        {
            Type = itemType;
            _inventorySlotStrategy = inventorySlotStrategy;
            SlotIndex = slotIndex;
            SetColor(itemType);
            _keyText.text = keyText;
        }

        public void SetColor(ItemType itemType)
        {
            Color color = itemType switch
            {
                ItemType.Weapon => _weaponColor,
                ItemType.Equipment => _equipmentColor,
                ItemType.Skin => _skinColor
            };

            _image.color =  color;
            _selectionImage.color = color;

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
            var item = eventData.pointerDrag.GetComponent<Item>();
            _inventorySlotStrategy.Drop(this,item);
        }
    }
}