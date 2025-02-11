using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Factories;
using App.Scripts.Modules.Localization;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponTabPresenter : InventoryTabPresenter
    {
        private readonly WeaponStatsView _statsView;
        private readonly SelectionProvider _selectionProvider;
        private readonly ILocalizationSystem _localizationSystem;

        private Dictionary<string,InventorySlot> _inventorySlots;
        
        public WeaponTabPresenter(InventoryTab view,
            WeaponStatsView statsView,
            SelectionProvider selectionProvider,
            IFactory<Item> itemFactory,
            IFactory<InventorySlot> slotFactory,
            InventoryProvider inventoryProvider,
            RectTransform overlayTransform,
            ILocalizationSystem localizationSystem) 
            : base(view, itemFactory, slotFactory,
            inventoryProvider, overlayTransform)
        {
            _statsView = statsView;
            _selectionProvider = selectionProvider;
            _localizationSystem = localizationSystem;
        }

        public override void Initialize()
        {
            _inventorySlots = new();
            _statsView.Initialize(_selectionProvider, InventoryProvider.GlobalInventory, _localizationSystem);
            InventoryProvider.Inventory.OnInventoryUpdated += UpdateInventory;
            UpdateInventory();
            
            _selectionProvider.Select(_inventorySlots.First().Value);
        }

        public override void Cleanup()
        {
            _statsView.Cleanup();
            InventoryProvider.Inventory.OnInventoryUpdated -= UpdateInventory;
        }
        
        private void UpdateInventory()
        {
            foreach (var weaponId in InventoryProvider.Inventory.Weapons)
            {
                if (_inventorySlots.ContainsKey(weaponId))
                {
                    continue;
                }
                
                var slot = SlotFactory.GetItem();
                slot.Initialize(new NoneInventorySlotStrategy(), -1);
                var item = ItemFactory.GetItem();
                var weaponConfig = InventoryProvider.WeaponById(weaponId);
                item.Initialize(_selectionProvider, OverlayTransform, weaponConfig.Sprite, weaponId);
                item.CurentSlot = slot;
                item.MoveToParent();
                View.AddSlot(slot);
                _inventorySlots.Add(weaponId, slot);
            }
        }
    }
}