using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Inventory
{
    public enum ItemType //TODO: Delete
    {
        Weapon,
        Equipment,
        Skin,
    }
    
    public class Item : MonoBehaviour, IBeginDragHandler , IEndDragHandler , IDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        private Transform _overlayParent;

        private InventorySlot _curentSlot;

        public InventorySlot CurentSlot
        {
            get => _curentSlot;
            set
            {
                _curentSlot = value;
                _curentSlot.Item = this;
            }
        }

        public string ConfigId { get; private set; }
        public ItemType Type { get; private set; }
        
        public void Initialize(Transform overlayParent, Sprite sprite, string id, ItemType type = ItemType.Weapon)
        {
            _overlayParent = overlayParent;
            _image.sprite = sprite;
            
            ConfigId = id;
            Type = type;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_overlayParent);
            _image.raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            MoveToParent();
            _image.raycastTarget = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
        
        public void MoveToParent()
        {
            transform.SetParent(CurentSlot.transform, false);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}