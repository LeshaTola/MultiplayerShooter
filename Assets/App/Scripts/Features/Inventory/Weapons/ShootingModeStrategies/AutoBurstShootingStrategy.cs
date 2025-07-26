using App.Scripts.Features.Inventory.Weapons.ShootStrategies;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class AutoBurstShootingStrategy : ShootingMode
    {
        [SerializeField] private int _burstCount = 3;
        [SerializeField] private float _burstCooldown = 0.5f;

        private int _currentCount;
        private bool _isCancelled;
        
        public override void StartAttack()
        {
            _isCancelled = false;
            
            if (IsShooting)
            {
                return;
            }

            _currentCount = _burstCount;
            base.StartAttack();
        }

        public override void PerformAttack()
        {
            if (_isCancelled)
            {
                if (_currentCount <= 0)
                {
                    GuaranteedCancelAttack();
                    Weapon.StartAttackCooldown(_burstCooldown);
                    return;
                }

                if (_currentCount == _burstCount)
                {
                    GuaranteedCancelAttack();
                    return;
                }
            }
            
            if (_currentCount <= 0)
            {
                Weapon.StartAttackCooldown(_burstCooldown);
                _currentCount = _burstCount;
                ShootStrategy.Recoil.IsShooting = false;
                return;
            }
            
            base.PerformAttack();
            if (!IsMissCounted && _isMiss)
            {
                return;
            }
            
            ShootStrategy.Recoil.IsShooting = true;

            ShootStrategy.Shoot();
            Weapon.Owner.AudioProvider.RPCPlayWeaponSound();
            Weapon.ChangeAmmoCount(-1);
            Weapon.Animator.AttackAnimation(AttackCooldown);
            Weapon.StartAttackCooldown(AttackCooldown);
            _currentCount--;
        }

        public override void CancelAttack()
        {
            _isCancelled = true;
        }

        public override void Import(IShootingModeStrategy original)
        {
            base.Import(original);
            var concreteOriginal = (AutoBurstShootingStrategy) original;

            _burstCount = concreteOriginal._burstCount;
            _burstCooldown = concreteOriginal._burstCooldown;
        }
    }
}