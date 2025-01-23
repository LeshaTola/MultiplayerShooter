using System;
using App.Scripts.Features.Inventory.Configs;

namespace App.Scripts.Scenes.MainMenu.Inventory.Slot.SelectionProviders
{
    public class SelectionProvider
    {
        public Action<string> OnWeaponSelected;
        public Action<string> OnSkinSelected;
        
        private InventorySlot _selectedWeapon;
        private InventorySlot _selectedSkin;

        public void Select(InventorySlot slot)
        {
            if (slot.Type == ItemType.Weapon)
            {
                _selectedWeapon?.Unselect();
                _selectedWeapon = slot;
                _selectedWeapon.Select();
                OnWeaponSelected?.Invoke(_selectedWeapon.Item.ConfigId);
            }

            if (slot.Type == ItemType.Skin)
            {
                _selectedSkin?.Unselect();
                _selectedSkin = slot;
                _selectedSkin.Select();
                OnSkinSelected?.Invoke(_selectedSkin.Item.ConfigId);
            }
        }
    }
}