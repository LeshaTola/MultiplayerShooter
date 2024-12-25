using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Weapon", fileName = "WeaponConfig")]
    public class WeaponConfig : ItemConfig
    {
        [field: SerializeField] public Weapon Prefab { get; private set; }
        [field: SerializeField] public int MaxAmmoCount { get; private set; }
        [field: SerializeField] public float ReloadCooldown { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public int Damage { get; } = 10;

        [field: HorizontalGroup(GroupID = "Attack")]
        [field: SerializeField]
        public IShootingModeStrategy ShootingMode { get; private set; }

        [field: HorizontalGroup(GroupID = "Alternative Attack")]
        [field: SerializeField]
        public IShootingModeStrategy ShootingModeAlternative { get; private set; }

        public void Initialize(IShootingModeStrategy shootingModeStrategy,
            IShootingModeStrategy shootingModeAlternativeStrategy)
        {
            ShootingMode = shootingModeStrategy;
            ShootingModeAlternative = shootingModeAlternativeStrategy;
        }
    }
}