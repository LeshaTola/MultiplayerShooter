namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Slot
{
    public interface IInventorySlotStrategy
    {
        public void Drop(InventorySlot inventorySlot, Item item);
    }
}