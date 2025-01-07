using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory
{
    [CreateAssetMenu(menuName = "Configs/Inventory/GlobalInventory", fileName = "GlobalInventory")]
    public class GlobalInventory: ScriptableObject
    {
        [field: SerializeField] public List<WeaponConfig> Weapons { get; private set; }
        [field: SerializeField] public List<EquipmentConfig> Equipment { get; private set; }
    }
}