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
        private readonly RaritiesDatabase _raritiesDatabase;
        
        private CorrectTypeInventorySlotStrategy _slotStrategy;

        public SkinsTabPresenter(InventoryTab view,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            InventorySlot skinSlot,
            UserStatsProvider userStatsProvider,
            GameInventoryView gameInventoryView,
            RaritiesDatabase raritiesDatabase)
            : base(view, itemFactory, slotFactory, inventoryProvider, overlayTransform)
        {
            _selectionProvider = selectionProvider;
            _skinSlot = skinSlot;
            _userStatsProvider = userStatsProvider;
            _gameInventoryView = gameInventoryView;
            _raritiesDatabase = raritiesDatabase;
        }
        
        private Dictionary<string,InventorySlot> _inventorySlots;

        public override void Initialize()
        {
            _inventorySlots = new();
            
            InventoryProvider.Inventory.OnInventoryUpdated += UpdateInventory;
            UpdateInventory();
            SetupSkinSlot();
            SpawnItem(InventoryProvider.SkinById(InventoryProvider.GameInventory.Skin), _skinSlot);
        }

        private void SetupSkinSlot()
        {
            _slotStrategy = new CorrectTypeInventorySlotStrategy(_selectionProvider,
                ItemType.Skin,
                InventoryProvider, 
                ItemFactory,
                _gameInventoryView);
            _slotStrategy.OnInventoryChanged += OnInventoryChanged;
            
            var color = Color.white;
            color.a = 0f;
            _skinSlot.Initialize(_slotStrategy, -1, color,$"", ItemType.Skin);
        }

        public override void Cleanup()
        {
            InventoryProvider.Inventory.OnInventoryUpdated -= UpdateInventory;
            _slotStrategy.OnInventoryChanged -= OnInventoryChanged;
        }

        public override async UniTask Show()
        {
            await base.Show();
            await View.Show();
            _selectionProvider.Select(_skinSlot);
        }

        private void UpdateInventory()
        {
            
            var skinConfigs 
                = InventoryProvider.Inventory.Skins.Select(weaponId => InventoryProvider.SkinById(weaponId)).ToList();
            var sortedItems = _raritiesDatabase.SortByRarity(skinConfigs.Cast<ItemConfig>().ToList());
            
            foreach (var skin in sortedItems)
            {
                if (_inventorySlots.ContainsKey(skin.Id))
                {
                    continue;
                }
                
                var slot = SlotFactory.GetItem();
                var item = ItemFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1, _raritiesDatabase.Rarities[skin.Rarity].Color,"", ItemType.Skin);
                item.Initialize(_selectionProvider, OverlayTransform, skin.Sprite, skin.Id, ItemType.Skin);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
                _inventorySlots.Add(skin.Id, slot);
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