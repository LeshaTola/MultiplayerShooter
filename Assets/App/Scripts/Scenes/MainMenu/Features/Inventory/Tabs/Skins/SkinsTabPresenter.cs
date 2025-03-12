using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins
{
    public class SkinsTabPresenter : InventoryTabPresenter
    {
        private readonly SelectionProvider _selectionProvider;
        private readonly InventorySlot _skinSlot;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly GameInventoryView _gameInventoryView;
        private CorrectTypeInventorySlotStrategy _slotStrategy;

        public SkinsTabPresenter(InventoryTab view,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            InventorySlot skinSlot,
            UserStatsProvider userStatsProvider,
            GameInventoryView gameInventoryView)
            : base(view, itemFactory, slotFactory, inventoryProvider, overlayTransform)
        {
            _selectionProvider = selectionProvider;
            _skinSlot = skinSlot;
            _userStatsProvider = userStatsProvider;
            _gameInventoryView = gameInventoryView;
        }
        
        private Dictionary<string,InventorySlot> _inventorySlots;

        public override void Initialize()
        {
            _inventorySlots = new();
            
            InventoryProvider.Inventory.OnInventoryUpdated += UpdateInventory;
            UpdateInventory();

            _slotStrategy = new CorrectTypeInventorySlotStrategy(_selectionProvider,
                ItemType.Skin,
                InventoryProvider, 
                ItemFactory,
                _gameInventoryView);
            _slotStrategy.OnInventoryChanged += OnInventoryChanged;
            
            _skinSlot.Initialize(_slotStrategy, -1, $"", ItemType.Skin);
            SpawnItem(InventoryProvider.SkinById(InventoryProvider.GameInventory.Skin), _skinSlot);
        }

        public override void Cleanup()
        {
            _slotStrategy.OnInventoryChanged -= OnInventoryChanged;
            InventoryProvider.Inventory.OnInventoryUpdated -= UpdateInventory;
        }

        public override async UniTask Show()
        {
            await base.Show();
            await View.Show();
            _selectionProvider.Select(_skinSlot);
        }

        private void UpdateInventory()
        {
            foreach (var skinId in InventoryProvider.Inventory.Skins)
            {
                if (_inventorySlots.ContainsKey(skinId))
                {
                    continue;
                }
                
                var slot = SlotFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1, "", ItemType.Skin);
                var item = ItemFactory.GetItem();
                var skinConfig = InventoryProvider.SkinById(skinId);
                item.Initialize(_selectionProvider, OverlayTransform, skinConfig.Sprite, skinId, ItemType.Skin);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
                _inventorySlots.Add(skinId, slot);
            }
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

        private void OnInventoryChanged()
        {
            _userStatsProvider.SaveState();
        }
    }
}