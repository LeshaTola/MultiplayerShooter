using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Modules.Localization;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponTabPresenter : InventoryTabPresenter
    {
        private readonly MarketView _statsView;
        private readonly SelectionProvider _selectionProvider;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly WeaponModelsUIProvider _weaponModelsUIProvider;
        private readonly PlayerModelsUIProvider _playerModelsUIProvider;
        private readonly RaritiesDatabase _raritiesDatabase;

        private Dictionary<string, InventorySlot> _inventorySlots;

        public WeaponTabPresenter(InventoryTab view,
            MarketView statsView,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            ILocalizationSystem localizationSystem,
            WeaponModelsUIProvider weaponModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider,
            RaritiesDatabase raritiesDatabase)
            : base(view, itemFactory, slotFactory,
                inventoryProvider, overlayTransform)
        {
            _statsView = statsView;
            _selectionProvider = selectionProvider;
            _localizationSystem = localizationSystem;
            _weaponModelsUIProvider = weaponModelsUIProvider;
            _playerModelsUIProvider = playerModelsUIProvider;
            _raritiesDatabase = raritiesDatabase;
        }

        public override void Initialize()
        {
            _inventorySlots = new ();
            _statsView.Initialize(_selectionProvider, InventoryProvider.GlobalInventory, _localizationSystem,
                _weaponModelsUIProvider, _playerModelsUIProvider);
            InventoryProvider.Inventory.OnInventoryUpdated += UpdateInventory;
            UpdateInventory();
        }

        public override void Cleanup()
        {
            _statsView.Cleanup();
            InventoryProvider.Inventory.OnInventoryUpdated -= UpdateInventory;
        }

        public override async UniTask Show()
        {
            await base.Show();
            await View.Show();
            _selectionProvider.Select(_inventorySlots.First().Value);
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