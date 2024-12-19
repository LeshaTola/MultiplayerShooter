namespace App.Scripts.Scenes.MainMenu.Inventory.Slot
{
    public interface IInventorySlotStrategy
    {
        public void Drop(InventorySlot inventorySlot, Item item);
    }
}