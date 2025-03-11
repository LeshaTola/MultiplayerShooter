using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory
{
    public class GameInventoryView : GameScreen
    {
        [SerializeField] private Transform _weaponsContainer;

        public List<InventorySlot> WeaponsSlots { get; } = new();

        public void AddWeaponSlot(InventorySlot inventorySlot)
        {
            inventorySlot.transform.SetParent(_weaponsContainer,false);
            WeaponsSlots.Add(inventorySlot);
        }

        public override void Cleanup()
        {
            foreach (Transform child in _weaponsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}