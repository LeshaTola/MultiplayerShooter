using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory
{
    public enum ItemType //TODO: Delete
    {
        Weapon,
        Equipment,
        Skin,
    }
    
    public class Item : MonoBehaviour, IBeginDragHandler , IEndDragHandler , IDragHandler, IPointerClickHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        private Transform _overlayParent;

        private InventorySlot _curentSlot;
        private SelectionProvider _selectionProvider;

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
        
        public void Initialize(SelectionProvider selectionProvider,
            Transform overlayParent,
            Sprite sprite,
            string id,
            ItemType type = ItemType.Weapon)
        {
            _selectionProvider = selectionProvider;
            _overlayParent = overlayParent;
            _image.sprite = sprite;
            
            ConfigId = id;
            Type = type;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_overlayParent,false);
            _image.raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            MoveToParent();
            _image.raycastTarget = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _rectTransform,
                eventData.position,
                Camera.main, 
                out Vector3 worldPoint);
            
            _rectTransform.position = worldPoint;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _selectionProvider.Select(_curentSlot);
        }

        public void MoveToParent()
        {
            transform.SetParent(CurentSlot.transform, false);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}