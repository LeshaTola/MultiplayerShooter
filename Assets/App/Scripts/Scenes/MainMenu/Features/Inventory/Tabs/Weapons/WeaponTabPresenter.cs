using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Modules.Localization;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponTabPresenter : InventoryTabPresenter
    {
        private readonly SelectionProvider _selectionProvider;

        private readonly RaritiesDatabase _raritiesDatabase;
        private readonly MarketViewPresenter _marketViewPresenter;

        private Dictionary<string, InventorySlot> _inventorySlots;

        public WeaponTabPresenter(InventoryTab view,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            RaritiesDatabase raritiesDatabase,MarketViewPresenter marketViewPresenter)
            : base(view, itemFactory, slotFactory,
                inventoryProvider, overlayTransform)
        {
            _selectionProvider = selectionProvider;
            _raritiesDatabase = raritiesDatabase;
            _marketViewPresenter = marketViewPresenter;
        }

        public override void Initialize()
        {
            _inventorySlots = new ();
            InventoryProvider.Inventory.OnInventoryUpdated += UpdateInventory;
            UpdateInventory();
        }

        public override void Cleanup()
        {
            InventoryProvider.Inventory.OnInventoryUpdated -= UpdateInventory;
        }

        public override async UniTask Show()
        {
            await base.Show();
            await View.Show();
            _selectionProvider.SelectWithoutNotification(_inventorySlots.First().Value);
            _marketViewPresenter.OnWeaponSelected(_inventorySlots.First().Value.Item.ConfigId);
        }

        private void UpdateInventory()
        {
            var weaponConfigs 
                = InventoryProvider.Inventory.Weapons.Select(weaponId => InventoryProvider.WeaponById(weaponId)).ToList();
            var sortedItems = _raritiesDatabase.SortByRarity(weaponConfigs.Cast<ItemConfig>().ToList());

            foreach (var weapon in sortedItems)
            {
                if (_inventorySlots.ContainsKey(weapon.Id))
                {
                    continue;
                }
                var slot = SlotFactory.GetItem();
                var item = ItemFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1,_raritiesDatabase.Rarities[weapon.Rarity].Color);
                item.Initialize(_selectionProvider, OverlayTransform, weapon.Sprite, weapon.Id);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
                _inventorySlots.Add(weapon.Id, slot);
            }
        }
    }
}