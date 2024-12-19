namespace App.Scripts.Scenes.MainMenu.Inventory.Slot
{
    public class DropInventorySlotStrategy:IInventorySlotStrategy
    {
        public void Drop(InventorySlot inventorySlot, Item item)
        {
            item.CurentSlot = inventorySlot;
        }
    }
}