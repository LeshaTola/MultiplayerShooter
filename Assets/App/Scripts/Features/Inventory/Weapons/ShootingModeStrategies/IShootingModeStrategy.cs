using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public interface IShootingModeStrategy
    {
        public IShootStrategy ShootStrategy { get; set; }
        
        public bool IsShooting { get;}

        public void Initialize(Weapon weapon);

        public void StartAttack();

        public void PerformAttack();

        public void CancelAttack();
        public void Import(IShootingModeStrategy original);
    }

    public abstract class ShootingMode : IShootingModeStrategy
    {
        [field: SerializeField] public IShootStrategy ShootStrategy { get; set; }

        protected Weapon Weapon;
        
        public bool IsShooting { get; protected set; }

        public virtual void Initialize(Weapon weapon)
        {
            ShootStrategy.Initialize(weapon);
            Weapon = weapon;
        }

        public virtual void StartAttack()
        {
            IsShooting = true;
            ShootStrategy.Recoil.IsShooting = true;
        }

        public virtual void PerformAttack()
        {
        }

        public virtual void CancelAttack()
        {
            IsShooting = false;
            ShootStrategy.Recoil.IsShooting = false;
        }

        public virtual void Import(IShootingModeStrategy original)
        {
            ShootStrategy.Import(original.ShootStrategy);
        }
    }
}