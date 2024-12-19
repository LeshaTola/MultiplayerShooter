using Zenject;

namespace App.Scripts.Scenes.MainMenu.Inventory.Slot.Factories
{
    public class SlotDropStrategyFactory
    {
        private readonly DiContainer _container;
        private readonly IInventorySlotStrategy _slotStrategy;
        
        public SlotDropStrategyFactory(DiContainer container, IInventorySlotStrategy slotStrategy)
        {
            _container = container;
            _slotStrategy = slotStrategy;
        }

        public void SetupSlot(int slotId, InventorySlot slot)
        {
            var slotType = _container.Instantiate(_slotStrategy.GetType()) as IInventorySlotStrategy;
            slot.Initialize(slotType,slotId);
        }
    }
}