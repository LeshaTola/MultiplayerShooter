using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory
{
    [CreateAssetMenu(menuName = "Configs/Inventory/GameInventory", fileName = "GameInventory")]
    public class GameInventory: ScriptableObject
    {
        [field: SerializeField] public int WeaponsCount { get; private set; }
        [field: SerializeField] public List<WeaponConfig> Weapons { get; private set; }
        //[field: SerializeField] public List<WeaponData> Weapons { get; private set; }
    }
}