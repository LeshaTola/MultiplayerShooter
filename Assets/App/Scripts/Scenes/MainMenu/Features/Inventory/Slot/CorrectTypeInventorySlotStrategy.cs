using System;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Slot
{
    public class CorrectTypeInventorySlotStrategy : IInventorySlotStrategy
    {
        public event Action OnInventoryChanged;
        
        private readonly ItemType _correctType;
        private readonly InventoryProvider _inventoryProvider;
        private readonly IFactory<Item> _itemFactory;
        private readonly GameInventoryView _gameInventoryView;
        private readonly SelectionProvider _selectionProvider;

        public CorrectTypeInventorySlotStrategy(SelectionProvider selectionProvider,
            ItemType correctType,
            InventoryProvider inventoryProvider,
            IFactory<Item> itemFactory,
            GameInventoryView gameInventoryView)
        {
            _selectionProvider = selectionProvider;
            _correctType = correctType;
            _inventoryProvider = inventoryProvider;
            _itemFactory = itemFactory;
            _gameInventoryView = gameInventoryView;
        }

        public void Drop(InventorySlot inventorySlot, Item item)
        {
            if (item.Type != _correctType)
            {
                return;
            }

            if (SwapItem(inventorySlot, item))
            {
                _selectionProvider.Select(inventorySlot);
                OnInventoryChanged?.Invoke();
                return;
            }

            if (inventorySlot.Item != null)
            {
                Object.Destroy(inventorySlot.Item.gameObject);
            }

            var config = _inventoryProvider.GetConfigById(item.ConfigId);
            var newItem = _itemFactory.GetItem();
            newItem.Initialize(_selectionProvider, item.transform.parent, config.Sprite, config.Id, item.Type);
            newItem.CurentSlot = inventorySlot;
            newItem.MoveToParent();
            _selectionProvider.Select(inventorySlot);

            switch (config)
            {
                case WeaponConfig weaponConfig:
                    _inventoryProvider.GameInventory.Weapons[inventorySlot.SlotIndex]
                        = weaponConfig.Id;
                    break;
                case EquipmentConfig equipmentConfig:
                    _inventoryProvider.GameInventory.Equipment[inventorySlot.SlotIndex]
                        = equipmentConfig.Id;
                    break;
                case SkinConfig skinConfig:
                    _inventoryProvider.GameInventory.Skin = skinConfig.Id;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            OnInventoryChanged?.Invoke();
        }

        private bool SwapItem(InventorySlot inventorySlot, Item item)
        {
            var weaponIndex
                = _inventoryProvider.GameInventory.Weapons
                    .FindIndex(x => x.Equals(item.ConfigId));
            var equipmentIndex
                = _inventoryProvider.GameInventory.Equipment
                    .FindIndex(x => x.Equals(item.ConfigId));

            if (weaponIndex == -1)
            {
                return false;
            }
            
            SwapPerformed(inventorySlot, item, _gameInventoryView.WeaponsSlots[weaponIndex].Item, ItemType.Weapon);
            return true;

        }

        private void SwapPerformed(InventorySlot inventorySlot, Item item, Item inventoryItem, ItemType type)
        {
            if (item.CurentSlot.SlotIndex == -1)
            {
                Swap(inventorySlot, inventoryItem, ItemType.Weapon);
                return;
            }
            
            Swap(inventorySlot, item, type);
        }

        private void Swap(InventorySlot inventorySlot, Item item, ItemType type)
        {
            var config = _inventoryProvider.GetConfigById(item.ConfigId);
            var swapItem = inventorySlot.Item;
            ItemConfig swapItemConfig = null;
            if (swapItem != null)
            {
                swapItemConfig = _inventoryProvider.GetConfigById(swapItem.ConfigId);
            }
            
            if (type == ItemType.Weapon)
            {
                _inventoryProvider.GameInventory.Weapons[item.CurentSlot.SlotIndex] = swapItemConfig?.Id ?? "";
                _inventoryProvider.GameInventory.Weapons[inventorySlot.SlotIndex] = config.Id ?? "";
            }
            else
            {
                _inventoryProvider.GameInventory.Equipment[item.CurentSlot.SlotIndex] = swapItemConfig?.Id ?? "";
                _inventoryProvider.GameInventory.Equipment[inventorySlot.SlotIndex] = config.Id ?? "";
            }

            if (swapItem != null)
            {
                swapItem.CurentSlot = item.CurentSlot;
                swapItem.MoveToParent();
            }
            else
            {
                item.CurentSlot.Item = null;
            }
            
            item.CurentSlot = inventorySlot;
            item.MoveToParent();
        }
    }
}