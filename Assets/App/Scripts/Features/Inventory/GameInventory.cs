using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory
{
    [CreateAssetMenu(menuName = "Configs/Inventory/GameInventory", fileName = "GameInventory")]
    public class GameInventory: ScriptableObject
    {
        
        [field: SerializeField] public SkinConfig Skin { get; set; }
        
        [field: SerializeField, Space] public int WeaponsCount { get; private set; }
        [field: SerializeField] public List<WeaponConfig> Weapons { get; private set; }
        
        [field: SerializeField, Space] public int EquipmentCount { get; private set; }
        [field: SerializeField] public List<EquipmentConfig> Equipment { get; private set; }
    }
}