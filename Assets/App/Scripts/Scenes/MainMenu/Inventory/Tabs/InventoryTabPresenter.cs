using App.Scripts.Features.Inventory;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class InventoryTabPresenter : GameScreenPresenter
    {
        private readonly InventoryTab _view;
        private readonly IFactory<Item> _itemFactory;
        private readonly IFactory<InventorySlot> _slotFactory;
        private readonly InventoryProvider _inventoryProvider;
        private readonly RectTransform _overlayTransform;

        public InventoryTabPresenter(InventoryTab view,
            IFactory<Item> itemFactory, 
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,RectTransform overlayTransform)
        {
            _view = view;
            _itemFactory = itemFactory;
            _slotFactory = slotFactory;
            _inventoryProvider = inventoryProvider;
            _overlayTransform = overlayTransform;
        }

        public override void Initialize()
        {
            foreach (var weapon in _inventoryProvider.GlobalInventory.Weapons)
            {
                var slot = _slotFactory.GetItem();
                var item = _itemFactory.GetItem();
                item.Initialize(_overlayTransform, weapon.Sprite, weapon.Id);
                item.CurrentParent = slot.transform;
                item.MoveToParent();
                _view.AddSlot(slot);
            }
        }

        public override void Cleanup()
        {
            _view.Cleanup();
        }
    }
}