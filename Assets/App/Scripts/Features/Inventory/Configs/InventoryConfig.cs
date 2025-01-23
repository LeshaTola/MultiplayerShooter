using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Inventory", fileName = "InventoryConfig")]
    public class InventoryConfig: ScriptableObject
    {
        [field: SerializeField] public GlobalInventory PlayerInventory { get; private set; }
        
        [field: SerializeField]
        [field: FoldoutGroup("Game Inventory")]
        public SkinConfig Skin { get; set; }
        
        [field: SerializeField] 
        [field: FoldoutGroup("Game Inventory")]
        public List<WeaponConfig> Weapons { get; private set; }
        
        [field: SerializeField]
        [field: FoldoutGroup("Game Inventory")]
        public List<EquipmentConfig> Equipment { get; private set; }
        
    }
}