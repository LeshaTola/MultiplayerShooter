﻿using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Inventory.GameInventory
{
    public class GameInventoryViewPresenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly GameInventoryView _view;
        private readonly InventoryProvider _inventoryProvider;
        private readonly IFactory<InventorySlot> _inventorySlotFactory;
        private readonly IFactory<Item> _itemFactory;
        private readonly RectTransform _overlayTransform;

        public GameInventoryViewPresenter(GameInventoryView view,
            InventoryProvider inventoryProvider,
            IFactory<InventorySlot> inventorySlotFactory,
            IFactory<Item> itemFactory, RectTransform overlayTransform)
        {
            _view = view;
            _inventoryProvider = inventoryProvider;
            _inventorySlotFactory = inventorySlotFactory;
            _itemFactory = itemFactory;
            _overlayTransform = overlayTransform;
        }

        public override void Initialize()
        {
            _view.Initialize();

            for (int i = 0; i < _inventoryProvider.GameInventory.WeaponsCount; i++)
            {
                var slot = _inventorySlotFactory.GetItem();
                slot.Initialize(
                    new CorrectTypeInventorySlotStrategy(ItemType.Weapon, _inventoryProvider, _itemFactory, _view), i, $"{i+1}");
                _view.AddWeaponSlot(slot);

                SpawnItem(_inventoryProvider.GameInventory.Weapons[i], slot);
            }

            for (int i = 0; i < _inventoryProvider.GameInventory.EquipmentCount; i++)
            {
                var slot = _inventorySlotFactory.GetItem();
                var key = i == 0 ? "Q" : "E";//TODO: REMOVE HARDCODE
                slot.Initialize(
                    new CorrectTypeInventorySlotStrategy(ItemType.Equipment, _inventoryProvider, _itemFactory, _view),
                    i,key, ItemType.Equipment);
                _view.AddEquipmentSlot(slot);

                SpawnItem(_inventoryProvider.GameInventory.Equipment[i], slot);
            }
        }

        private void SpawnItem(ItemConfig itemConfig, InventorySlot slot)
        {
            if (!itemConfig)
            {
                return;
            }

            var type = itemConfig is WeaponConfig ? ItemType.Weapon : ItemType.Equipment;

            var item = _itemFactory.GetItem();
            item.Initialize(_overlayTransform, itemConfig.Sprite, itemConfig.Id, type);
            item.CurentSlot = slot;
            item.MoveToParent();
        }

        public override void Cleanup()
        {
            _view.Cleanup();
        }
    }
}