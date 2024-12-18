using System;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies;
using App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Weapon", fileName = "WeaponConfig")]
    public class WeaponConfig : SerializedScriptableObject
    {
        
        [field: SerializeField] 
        [field: ReadOnly] 
        public string Id { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        [field: SerializeField] public Weapon Prefab { get; private set; }

        [field: SerializeField] public int MaxAmmoCount { get; private set; }
        [field: SerializeField] public float ReloadCooldown { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public float Damage { get; } = 10;
        [field: SerializeField] public IShootStrategy ShootStrategy { get; private set; }
        [field: SerializeField] public IShootingModeStrategy ShootingMode { get; private set; }

        private void OnValidate()
        {
            Id = name;
        }
    }
}