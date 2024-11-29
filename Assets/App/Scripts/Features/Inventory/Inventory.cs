using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Inventory", fileName = "Inventory")]
    public class Inventory: ScriptableObject
    {
        [field: SerializeField] public List<WeaponData> Weapons { get; private set; }
        //[field: SerializeField] public List<WeaponData> Weapons { get; private set; }
    }
}