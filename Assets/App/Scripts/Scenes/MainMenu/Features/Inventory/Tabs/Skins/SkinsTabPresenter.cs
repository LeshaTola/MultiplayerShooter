using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins
{
    public class SkinsTabPresenter : InventoryTabPresenter
    {
        private readonly SelectionProvider _selectionProvider;
        private readonly InventorySlot _skinSlot;
        private readonly GameInventoryView _gameInventoryView;
        private readonly SkinsView _skinsView;

        public SkinsTabPresenter(InventoryTab view,
            SkinsView skinsView,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            InventorySlot skinSlot,
            GameInventoryView gameInventoryView)
            : base(view, itemFactory, slotFactory, inventoryProvider, overlayTransform)
        {
            _skinsView = skinsView;
            _selectionProvider = selectionProvider;
            _skinSlot = skinSlot;
            _gameInventoryView = gameInventoryView;
        }

        public override void Initialize()
        {
            _skinsView.Initialize(_selectionProvider, InventoryProvider.GlobalInventory);
            
            foreach (var skinId in InventoryProvider.Inventory.Skins)
            {
                var slot = SlotFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1, "", ItemType.Skin);
                var item = ItemFactory.GetItem();
                var skinConfig = InventoryProvider.SkinById(skinId);
                item.Initialize(_selectionProvider, OverlayTransform, skinConfig.Sprite, skinId, ItemType.Skin);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
            }
            
            _skinSlot.Initialize(
                new CorrectTypeInventorySlotStrategy(_selectionProvider ,ItemType.Skin, InventoryProvider, ItemFactory,
                    _gameInventoryView), -1, $"", ItemType.Skin);
            SpawnItem(InventoryProvider.SkinById(InventoryProvider.GameInventory.Skin), _skinSlot);
            
            _selectionProvider.Select(_skinSlot);
        }
        
        private void SpawnItem(ItemConfig itemConfig, InventorySlot slot)
        {
            if (!itemConfig)
            {
                return;
            }

            var item = ItemFactory.GetItem();
            item.Initialize(_selectionProvider, OverlayTransform, itemConfig.Sprite, itemConfig.Id, ItemType.Skin);
            item.CurentSlot = slot;
            item.MoveToParent();
        }

        public override void Cleanup()
        {
            _skinsView.Cleanup();
        }
    }
}