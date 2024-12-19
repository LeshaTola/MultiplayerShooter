using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.MainMenu.Inventory.Slot
{
    public class CorrectTypeInventorySlotStrategy : IInventorySlotStrategy
    {
        private readonly ItemType _correctType;
        private readonly InventoryProvider _inventoryProvider;
        private readonly IFactory<Item> _itemFactory;
        private readonly GameInventoryView _gameInventoryView;

        public CorrectTypeInventorySlotStrategy(ItemType correctType,
            InventoryProvider inventoryProvider,
            IFactory<Item> itemFactory,
            GameInventoryView gameInventoryView)
        {
            _correctType = correctType;
            _inventoryProvider = inventoryProvider;
            _itemFactory = itemFactory;
            _gameInventoryView = gameInventoryView;
        }

        public void Drop(InventorySlot inventorySlot, Item item)
        {
            if (SwapItem(inventorySlot, item)) return;

            if (item.Type != _correctType)
            {
                return;
            }

            if (inventorySlot.Item != null)
            {
                Object.Destroy(inventorySlot.Item.gameObject);
            }

            var config = _inventoryProvider.GetConfigById(item.ConfigId);
            var newItem = _itemFactory.GetItem();
            newItem.Initialize(item.transform.parent, config.Sprite, config.Id);
            newItem.CurentSlot = inventorySlot;
            newItem.MoveToParent();

            switch (config)
            {
                case WeaponConfig weaponConfig:
                    _inventoryProvider.GameInventory.Weapons[inventorySlot.SlotIndex]
                        = weaponConfig;
                    break;
                case EquipmentConfig equipmentConfig:
                    _inventoryProvider.GameInventory.Equipment[inventorySlot.SlotIndex]
                        = equipmentConfig;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SwapItem(InventorySlot inventorySlot, Item item)
        {
            var weaponIndex
                = _inventoryProvider.GameInventory.Weapons
                    .FindIndex(x => x && x.Id.Equals(item.ConfigId));
            var equipmentIndex
                = _inventoryProvider.GameInventory.Equipment
                    .FindIndex(x => x && x.Id.Equals(item.ConfigId));

            if (weaponIndex != -1)
            {
                Swap(inventorySlot, item, weaponIndex, _gameInventoryView.WeaponsSlots);
                return true;
            }

            if (equipmentIndex != -1)
            {
                Swap(inventorySlot, item, weaponIndex, _gameInventoryView.EquipmentSlots, ItemType.Equipment);
                return true;
            }

            return false;
        }

        private void Swap(InventorySlot inventorySlot, Item item, int slotIndex, List<InventorySlot> slots,
            ItemType type = ItemType.Weapon)
        {
            var config = _inventoryProvider.GetConfigById(item.ConfigId);
            var swapItem = slots[slotIndex].Item;
            if (item.CurentSlot.SlotIndex == -1)
            {
                if (inventorySlot.Item != null)
                {
                    aaaa(inventorySlot, inventorySlot.Item,ItemType.Weapon,config);
                    return;
                }
                
                if (type == ItemType.Weapon)
                {
                    _inventoryProvider.GameInventory.Weapons[inventorySlot.SlotIndex] = config as WeaponConfig;
                    _inventoryProvider.GameInventory.Weapons[swapItem.CurentSlot.SlotIndex] = null;
                }
                else
                {
                    _inventoryProvider.GameInventory.Equipment[inventorySlot.SlotIndex] = config as EquipmentConfig;
                    _inventoryProvider.GameInventory.Equipment[swapItem.CurentSlot.SlotIndex] = null;
                }

                swapItem.CurentSlot = inventorySlot;
                swapItem.MoveToParent();
                return;
            }
            
            aaaa(inventorySlot, item, type, config);
        }

        private void aaaa(InventorySlot inventorySlot, Item item, ItemType type, ItemConfig config)
        {
            Item swapItem;
            swapItem = inventorySlot.Item;
            ItemConfig swapItemConfig = null;
            if (swapItem != null)
            {
                swapItemConfig = _inventoryProvider.GetConfigById(swapItem.ConfigId);
            }
            if (type == ItemType.Weapon)
            {
                _inventoryProvider.GameInventory.Weapons[item.CurentSlot.SlotIndex] = swapItemConfig as WeaponConfig;
                _inventoryProvider.GameInventory.Weapons[inventorySlot.SlotIndex] = config as WeaponConfig;
            }
            else
            {
                _inventoryProvider.GameInventory.Equipment[item.CurentSlot.SlotIndex] = swapItemConfig as EquipmentConfig;
                _inventoryProvider.GameInventory.Equipment[inventorySlot.SlotIndex] = config as EquipmentConfig;
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