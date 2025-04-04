﻿using System;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders
{
    public class SelectionProvider
    {
        public event Action<string> OnWeaponSelected;
        public event Action<int> OnWeaponSelectedSlotId;
        public event Action<string> OnSkinSelected;
        
        private InventorySlot _selectedWeapon;
        private InventorySlot _selectedSkin;

        public void Select(InventorySlot slot)
        {
            SelectWithoutNotification(slot);
            if (slot.Type == ItemType.Weapon)
            {
                OnWeaponSelected?.Invoke(_selectedWeapon.Item.ConfigId);
                OnWeaponSelectedSlotId?.Invoke(_selectedWeapon.SlotIndex);
            }

            if (slot.Type == ItemType.Skin)
            {
                OnSkinSelected?.Invoke(_selectedSkin.Item.ConfigId);
            }
        }
        
        public void SelectWithoutNotification(InventorySlot slot)
        {
            if (slot.Type == ItemType.Weapon)
            {
                _selectedWeapon?.Unselect();
                _selectedWeapon = slot;
                _selectedWeapon.Select();
            }

            if (slot.Type == ItemType.Skin)
            {
                _selectedSkin?.Unselect();
                _selectedSkin = slot;
                _selectedSkin.Select();
            }
        }
    }
}