using App.Scripts.Features.Inventory.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Inventory
{
    public enum ItemType //TODO: Delete
    {
        Weapon,
        Equipment,
    }
    
    public class Item : MonoBehaviour, IBeginDragHandler , IEndDragHandler , IDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        private RectTransform _overlayParent;

        public Transform CurrentParent { get; set; }
        public string ConfigId { get; private set; }
        public ItemType Type { get; private set; }
        
        public void Initialize(RectTransform overlayParent, Sprite sprite, string id, ItemType type = ItemType.Equipment)
        {
            _overlayParent = overlayParent;
            _image.sprite = sprite;
            
            ConfigId = id;
            Type = type;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            CurrentParent = transform.parent;
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
            transform.SetParent(CurrentParent);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}