﻿using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class SkinsTabPresenter : InventoryTabPresenter
    {
        private readonly InventorySlot _skinSlot;
        private readonly GameInventoryView _gameInventoryView;

        public SkinsTabPresenter(InventoryTab view,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            InventorySlot skinSlot,
            GameInventoryView gameInventoryView)
            : base(view, itemFactory, slotFactory, inventoryProvider, overlayTransform)
        {
            _skinSlot = skinSlot;
            _gameInventoryView = gameInventoryView;
        }

        public override void Initialize()
        {
            foreach (var weapon in InventoryProvider.GlobalInventory.SkinConfigs)
            {
                var slot = SlotFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1, "", ItemType.Skin);
                var item = ItemFactory.GetItem();
                item.Initialize(OverlayTransform, weapon.Sprite, weapon.Id, ItemType.Skin);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
            }

            _skinSlot.Initialize(
                new CorrectTypeInventorySlotStrategy(ItemType.Skin, InventoryProvider, ItemFactory,
                    _gameInventoryView), -1, $"", ItemType.Skin);
            SpawnItem(InventoryProvider.GameInventory.Skin, _skinSlot);
        }
        
        private void SpawnItem(ItemConfig itemConfig, InventorySlot slot)
        {
            if (!itemConfig)
            {
                return;
            }

            var item = ItemFactory.GetItem();
            item.Initialize(OverlayTransform, itemConfig.Sprite, itemConfig.Id, ItemType.Skin);
            item.CurentSlot = slot;
            item.MoveToParent();
        }
    }
}