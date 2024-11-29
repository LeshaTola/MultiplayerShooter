using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Weapon", fileName = "WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Weapon _weaponPrefab;
        

    }
    
    
}