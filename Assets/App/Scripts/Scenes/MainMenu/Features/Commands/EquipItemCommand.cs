using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.Factories;
using App.Scripts.Scenes.MainMenu.Features.Inventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Commands
{
    public class EquipItemCommand : LabeledCommand
    {
        private readonly InventoryProvider _inventoryProvider;

        public SkinConfig Skin { get; set; }
        
        public EquipItemCommand(string label, InventoryProvider inventoryProvider) : base(label)
        {
            _inventoryProvider = inventoryProvider;
        }

        public override void Execute()
        {
            _inventoryProvider.GameInventory.Skin = Skin.Id;
            _inventoryProvider.GameInventory.InvokeInventoryUpdate();
        }
    }
}