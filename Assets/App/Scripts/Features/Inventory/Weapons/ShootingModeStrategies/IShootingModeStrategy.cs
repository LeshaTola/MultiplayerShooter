using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootEffects;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public interface IShootingModeStrategy
    {
        public float AttackCooldown { get; }

        public IShootStrategy ShootStrategy { get; set; }
        
        public List<ShootEffect> ShootEffects {get;set;}

        public bool IsShooting { get;}
        public bool IsMissCounted { get;}
        

        public void Initialize(Weapon weapon);

        public void StartAttack();

        public void PerformAttack();

        public void CancelAttack();

        public void GuaranteedCancelAttack();
        
        public void Import(IShootingModeStrategy original);
    }

    public abstract class ShootingMode : IShootingModeStrategy
    {
        [field: SerializeField] public float AttackCooldown { get; set; }
        [field: SerializeField] public IShootStrategy ShootStrategy { get; set; }
        [field: SerializeField] public List<ShootEffect> ShootEffects {get;set;}
        [field: SerializeField] public bool IsMissCounted { get; private set; } = true;

        protected Weapon Weapon;
        protected bool _isMiss;

        public bool IsShooting { get; protected set; }

        public virtual void Initialize(Weapon weapon)
        {
            foreach (var shootEffect in ShootEffects)
            {
                shootEffect.Initialize(weapon, this);
            }
            ShootStrategy.Initialize(weapon);
            Weapon = weapon;
        }

        public virtual void StartAttack()
        {
            if (Weapon.Owner != null)
            {
                Weapon.Owner.PlayerVisual.ShootAnimation(true,AttackCooldown);
            }
            
            IsShooting = true;
            ShootStrategy.Recoil.IsShooting = true;
        }

        public virtual void PerformAttack()
        {
            _isMiss = ShootStrategy.GetRaycastHit().Item1.collider == null;
            if (!IsMissCounted && _isMiss)
            {
                return;
            }
            
            foreach (var shootEffect in ShootEffects)
            {
                shootEffect.Effect();
            }
        }

        public virtual void CancelAttack()
        {
            Weapon.Owner.PlayerVisual.ShootAnimation(false, AttackCooldown);
            IsShooting = false;
            ShootStrategy.Recoil.IsShooting = false;
        }

        public virtual void GuaranteedCancelAttack()
        {
            IsShooting = false;
            ShootStrategy.Recoil.IsShooting = false;
        }

        public virtual void Import(IShootingModeStrategy original)
        {
            IsMissCounted = original.IsMissCounted;
            AttackCooldown = original.AttackCooldown;
            ShootStrategy.Import(original.ShootStrategy);
            for (int i = 0; i < ShootEffects.Count; i++)
            {
                ShootEffects[i].Import(original.ShootEffects[i]);
            }
        }
    }
}