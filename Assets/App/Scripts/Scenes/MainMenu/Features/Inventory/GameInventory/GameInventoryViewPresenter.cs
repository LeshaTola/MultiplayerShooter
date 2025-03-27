using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.HUD.PlayerUI.Provider;
using App.Scripts.Scenes.Gameplay.HUD.PlayerUI.View;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory
{
    public class GameInventoryViewPresenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private GameInventoryView _view;
        private readonly SelectionProvider _selectionProvider;
        private readonly InventoryProvider _inventoryProvider;
        private readonly IFactory<InventorySlot> _inventorySlotFactory;
        private readonly IFactory<Item> _itemFactory;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly RectTransform _overlayTransform;
        private readonly PlayerUIProvider _playerUIProvider;

        private readonly List<CorrectTypeInventorySlotStrategy> _subscriptionsSlots = new();
        
        public GameInventoryViewPresenter(
            SelectionProvider selectionProvider,
            InventoryProvider inventoryProvider,
            IFactory<InventorySlot> inventorySlotFactory,
            IFactory<Item> itemFactory,
            UserStatsProvider userStatsProvider,
            RectTransform overlayTransform,
            PlayerUIProvider playerUIProvider = null,
            GameInventoryView view = null)
        {
            _view = view;
            _selectionProvider = selectionProvider;
            _inventoryProvider = inventoryProvider;
            _inventorySlotFactory = inventorySlotFactory;
            _itemFactory = itemFactory;
            _userStatsProvider = userStatsProvider;
            _overlayTransform = overlayTransform;
            _playerUIProvider = playerUIProvider;
        }

        public override void Initialize()
        {
            if (_view == null)
            {
                var inventoryView = _playerUIProvider.PlayerView.InventoryView;
                if (inventoryView == null)
                {
                    _playerUIProvider.OnPlayerViewCreated += OnPlayerViewCreated;
                    return;
                }

                _view = inventoryView;
            }
            InitializeInternal();
        }

        public override void Cleanup()
        {
            foreach (var slot in _subscriptionsSlots)
            {
                slot.OnInventoryChanged -= OnInventoryChanged;
            }
            _subscriptionsSlots.Clear();
            _view.Cleanup();
        }

        public void SelectSlot(int index)
        {
            foreach (var slot in _view.WeaponsSlots)
            {
                slot.Unselect();   
            }
            _view.WeaponsSlots[index].Select();
        }

        private void InitializeWeapons()
        {
            for (int i = 0; i < _inventoryProvider.GameInventory.Weapons.Count; i++)
            {
                var slot = GetSlot(i);
                
                _view.AddWeaponSlot(slot);
                var weaponConfig = _inventoryProvider.WeaponById(_inventoryProvider.GameInventory.Weapons[i]);
                SpawnItem(weaponConfig, slot);
            }
        }

        private InventorySlot GetSlot(int i)
        {
            var slot = _inventorySlotFactory.GetItem();
            var slotStrategy
                = new CorrectTypeInventorySlotStrategy(_selectionProvider,
                    ItemType.Weapon,
                    _inventoryProvider,
                    _itemFactory, _view);
            slotStrategy.OnInventoryChanged += OnInventoryChanged;
            _subscriptionsSlots.Add(slotStrategy);

            var color = Color.white;
            color.a = 0f;
            slot.Initialize(slotStrategy, i, color,$"{i + 1}");
            return slot;
        }

        private void SpawnItem(ItemConfig itemConfig, InventorySlot slot)
        {
            if (!itemConfig)
            {
                return;
            }

            var type = itemConfig is WeaponConfig ? ItemType.Weapon : ItemType.Equipment;

            var item = _itemFactory.GetItem();
            item.Initialize(_selectionProvider, _overlayTransform, itemConfig.Sprite, itemConfig.Id, type);
            item.CurentSlot = slot;
            item.MoveToParent();
        }
        
        private void OnInventoryChanged()
        {
            _userStatsProvider.SaveState();
        }


        private void OnPlayerViewCreated(PlayerView playerView)
        {
            _view = playerView.InventoryView;
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            _view.Initialize();
            InitializeWeapons();
        }
    }
}