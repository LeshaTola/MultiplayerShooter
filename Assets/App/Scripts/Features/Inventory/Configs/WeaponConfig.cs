using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Configs
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Weapon", fileName = "WeaponConfig")]
    public class WeaponConfig : ItemConfig
    {
        [field: Header("View")]
        [field: SerializeField] public Weapon Prefab { get; private set; }
        [field: SerializeField] public Vector3 ViewOffset { get; private set; }
        [field: SerializeField] public Vector3 ViewRotationOffset { get; private set; }
        [field: SerializeField] public Vector3 ViewScaleMultiplier { get; private set; } = Vector3.one;

        
        [field: Header("Audio")]
        [field: SerializeField] public AudioClip ShotSound { get; private set; }
        [field: SerializeField] public AudioClip ReloadSound { get; private set; }
        
        [field: Header("General")]
        [field: SerializeField] public int MaxAmmoCount { get; private set; }
        [field: SerializeField] public float ReloadCooldown { get; private set; }
        
        [field: SerializeField] public List<(string, string)> Stats { get; private set; }


        [field: HorizontalGroup(GroupID = "Attack")]
        [field: Header("Shooting Modes")]
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