using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Scenes.MainMenu.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<Item>();
            item.CurrentParent = transform;
        }
    }
}