using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
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
            PlayerModelsUIProvider playerModelsUIProvider)
            : base(view, itemFactory, slotFactory,
                inventoryProvider, overlayTransform)
        {
            _statsView = statsView;
            _selectionProvider = selectionProvider;
            _localizationSystem = localizationSystem;
            _weaponModelsUIProvider = weaponModelsUIProvider;
            _playerModelsUIProvider = playerModelsUIProvider;
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