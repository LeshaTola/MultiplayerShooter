using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory
{
    public class GameInventoryView : GameScreen
    {
        [SerializeField] private Transform _weaponsContainer;
        [SerializeField] private Transform _equipmentContainer;

        public List<InventorySlot> WeaponsSlots { get; } = new();
        public List<InventorySlot> EquipmentSlots { get; } = new();

        public void AddWeaponSlot(InventorySlot inventorySlot)
        {
            inventorySlot.transform.SetParent(_weaponsContainer,false);
            WeaponsSlots.Add(inventorySlot);
        }
        
        public void AddEquipmentSlot(InventorySlot inventorySlot)
        {
            inventorySlot.transform.SetParent(_equipmentContainer,false);
            EquipmentSlots.Add(inventorySlot);
        }

        public override void Cleanup()
        {
            foreach (Transform child in _weaponsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in _equipmentContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}