using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class BurstShootingStrategy : ShootingMode
    {
        [SerializeField] private int _burstCount = 3;

        private int _currentCount;

        public override void StartAttack()
        {
            if (IsShooting)
            {
                return;
            }
            
            _currentCount = _burstCount;
            base.StartAttack();
        }

        public override void PerformAttack()
        {
            if (_currentCount <= 0)
            {
                IsShooting = false;
                ShootStrategy.Recoil.IsShooting = false;
                return;
            }

            base.PerformAttack();
            if (!IsMissCounted && _isMiss)
            {
                return;
            }

            ShootStrategy.Shoot();
            Weapon.Owner.PlayerAudioProvider.RPCPlayWeaponSound();
            
            Weapon.ChangeAmmoCount(-1);
            Weapon.Animator.AttackAnimation(AttackCooldown);
            Weapon.StartAttackCooldown(AttackCooldown);
            _currentCount--;
        }

        public override void CancelAttack()
        {
        }

        public override void Import(IShootingModeStrategy original)
        {
            base.Import(original);
            var concreteOriginal = (BurstShootingStrategy) original;

            _burstCount = concreteOriginal._burstCount;
        }
    }
}