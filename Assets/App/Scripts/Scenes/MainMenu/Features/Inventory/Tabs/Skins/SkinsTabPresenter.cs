using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins
{
    public class SkinsTabPresenter : InventoryTabPresenter
    {
        private readonly SelectionProvider _selectionProvider;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly GameInventoryView _gameInventoryView;
        private readonly RaritiesDatabase _raritiesDatabase;
        
        private CorrectTypeInventorySlotStrategy _slotStrategy;
        
        public InventorySlot SkinSlot { get; }

        public SkinsTabPresenter(InventoryTab view,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            InventorySlot skinSlot,
            UserStatsProvider userStatsProvider,
            GameInventoryView gameInventoryView,
            RaritiesDatabase raritiesDatabase, MarketViewPresenter marketViewPresenter)
            : base(view, itemFactory, slotFactory, inventoryProvider, overlayTransform)
        {
            _selectionProvider = selectionProvider;
            SkinSlot = skinSlot;
            _userStatsProvider = userStatsProvider;
            _gameInventoryView = gameInventoryView;
            _raritiesDatabase = raritiesDatabase;
            _marketViewPresenter = marketViewPresenter;
        }
        
        private Dictionary<string,InventorySlot> _inventorySlots;
        private readonly MarketViewPresenter _marketViewPresenter;

        public override void Initialize()
        {
            _inventorySlots = new();

            InventoryProvider.GameInventory.OnInventoryUpdated += UpdateGameInventory;
            InventoryProvider.Inventory.OnInventoryUpdated += UpdateInventory;
            UpdateInventory();
            SetupSkinSlot();
            UpdateGameInventory();

        }

        private void UpdateGameInventory()
        {
            SpawnItem(InventoryProvider.SkinById(InventoryProvider.GameInventory.Skin) ,SkinSlot);
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
            SkinSlot.Initialize(_slotStrategy, -1, color,$"", ItemType.Skin);
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
            _selectionProvider.SelectWithoutNotification(SkinSlot);
            _marketViewPresenter.OnSkinSelected(SkinSlot.Item.ConfigId);
        }


        private void UpdateInventory()
        {
            var configs = GetAvailableWeapons();
            UpdateOpenedSkins(configs);
            UpdateBlockedSkins(configs);
        }

        private void UpdateOpenedSkins(List<SkinConfig> skinConfigs)
        {
            var sortedItems = GetSortedWeapons(skinConfigs);

            foreach (var weapon in sortedItems)
            {
                if (_inventorySlots.TryGetValue(weapon.Id, out InventorySlot slot))
                {
                    slot.Item.SetBlocked(false);
                    continue;
                }

                AddItem(weapon, false);
            }
        }

        private void UpdateBlockedSkins(List<SkinConfig>  skinsConfigs)
        {
            var blockedWeapons = InventoryProvider.GlobalInventory.SkinConfigs.Except(skinsConfigs).ToList();
            var sortedBlockedWeapons = GetSortedWeapons(blockedWeapons);
            foreach (var weapon in sortedBlockedWeapons)
            {
                if (_inventorySlots.TryGetValue(weapon.Id, out InventorySlot slot))
                {
                    slot.Item.SetBlocked(true);
                    continue;
                }

                AddItem(weapon, true);
            }
        }

        private List<ItemConfig> GetSortedWeapons(List<SkinConfig>  skinsConfigs)
        {
            return _raritiesDatabase.SortByRarity(skinsConfigs.Cast<ItemConfig>().ToList());
        }

        private List<SkinConfig>GetAvailableWeapons()
        {
            return InventoryProvider.Inventory.Skins
                .Select(id => InventoryProvider.SkinById(id)).ToList();
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

        private void AddItem(ItemConfig skin, bool blocked)
        {
            var slot = SlotFactory.GetItem();
            var item = ItemFactory.GetItem();
            slot.Initialize(new NoneInventorySlotStrategy(), -1, _raritiesDatabase.Rarities[skin.Rarity].Color,"", ItemType.Skin);
            item.Initialize(_selectionProvider, OverlayTransform, skin.Sprite, skin.Id, ItemType.Skin);
            item.CurentSlot = slot;
            slot.Item.SetBlocked(blocked);
            item.MoveToParent();
            View.AddSlot(slot);
            _inventorySlots.Add(skin.Id, slot);
        }
    }
}