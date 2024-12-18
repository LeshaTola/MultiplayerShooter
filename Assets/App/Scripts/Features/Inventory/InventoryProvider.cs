namespace App.Scripts.Features.Inventory
{
    public class InventoryProvider
    {
        public GameInventory GameInventory { get; }
        public GlobalInventory GlobalInventory { get; }

        public InventoryProvider(GameInventory gameInventory, GlobalInventory globalInventory)
        {
            GameInventory = gameInventory;
            GlobalInventory = globalInventory;
        }
    }
}