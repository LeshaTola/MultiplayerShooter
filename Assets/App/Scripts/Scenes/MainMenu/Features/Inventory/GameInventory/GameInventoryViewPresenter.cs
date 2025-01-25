using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Factories;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory
{
    public class GameInventoryViewPresenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly GameInventoryView _view;
        private readonly SelectionProvider _selectionProvider;
        private readonly InventoryProvider _inventoryProvider;
        private readonly IFactory<InventorySlot> _inventorySlotFactory;
        private readonly IFactory<Item> _itemFactory;
        private readonly RectTransform _overlayTransform;

        public GameInventoryViewPresenter(GameInventoryView view,
            SelectionProvider selectionProvider,
            InventoryProvider inventoryProvider,
            IFactory<InventorySlot> inventorySlotFactory,
            IFactory<Item> itemFactory, RectTransform overlayTransform)
        {
            _view = view;
            _selectionProvider = selectionProvider;
            _inventoryProvider = inventoryProvider;
            _inventorySlotFactory = inventorySlotFactory;
            _itemFactory = itemFactory;
            _overlayTransform = overlayTransform;
        }

        public override void Initialize()
        {
            _view.Initialize();

            InitializeWeapons();
            InitializeEquipment();
        }

        public override void Cleanup()
        {
            _view.Cleanup();
        }

        private void InitializeWeapons()
        {
            for (int i = 0; i < _inventoryProvider.GameInventory.Weapons.Count; i++)
            {
                var slot = _inventorySlotFactory.GetItem();
                slot.Initialize(
                    new CorrectTypeInventorySlotStrategy(_selectionProvider, ItemType.Weapon, _inventoryProvider, _itemFactory, _view), i, $"{i+1}");
                _view.AddWeaponSlot(slot);
                var weaponConfig = _inventoryProvider.WeaponById(_inventoryProvider.GameInventory.Weapons[i]);
                SpawnItem(weaponConfig, slot);
            }
        }

        private void InitializeEquipment()
        {
            for (int i = 0; i < _inventoryProvider.GameInventory.Equipment.Count; i++)
            {
                var slot = _inventorySlotFactory.GetItem();
                var key = i == 0 ? "Q" : "E";//TODO: REMOVE HARDCODE
                slot.Initialize(
                    new CorrectTypeInventorySlotStrategy(_selectionProvider, ItemType.Equipment, _inventoryProvider, _itemFactory, _view),
                    i,key, ItemType.Equipment);
                _view.AddEquipmentSlot(slot);
                var equipmentConfig = _inventoryProvider.EquipmentById(_inventoryProvider.GameInventory.Equipment[i]);
                SpawnItem(equipmentConfig, slot);
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
            item.Initialize(_selectionProvider, _overlayTransform, itemConfig.Sprite, itemConfig.Id, type);
            item.CurentSlot = slot;
            item.MoveToParent();
        }
    }
}