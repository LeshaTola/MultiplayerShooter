namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Slot
{
    public class DropInventorySlotStrategy:IInventorySlotStrategy
    {
        public void Drop(InventorySlot inventorySlot, Item item)
        {
            item.CurentSlot = inventorySlot;
        }
    }
}