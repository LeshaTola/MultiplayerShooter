using System;
using App.Scripts.Features.Inventory.Weapons.ShootingRecoil;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies
{
    public interface IShootStrategy
    {
        public event Action<Vector3, float> OnPlayerHit;
        public Recoil Recoil { get; }

        public void Initialize(Weapon weapon);
        public void Shoot();

        public void Import(IShootStrategy original);
    }

    public abstract class ShootStrategy: IShootStrategy
    {
        public virtual event Action<Vector3, float> OnPlayerHit;

        [SerializeField] public Recoil Recoil { get; private set; } = new();
        
        protected Weapon Weapon;
        
        public virtual void Initialize(Weapon weapon)
        {
            Weapon = weapon;
        }

        public virtual void Shoot()
        {
            Recoil.Add();
        }

        public virtual void Import(IShootStrategy original)
        {
            Recoil.Import(original.Recoil);
        }
    }
}