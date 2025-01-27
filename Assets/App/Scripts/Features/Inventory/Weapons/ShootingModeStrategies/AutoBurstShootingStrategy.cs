using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class AutoBurstShootingStrategy : ShootingMode
    {
        [SerializeField] private int _burstCount = 3;
        [SerializeField] private float _burstCooldown = 0.5f;

        private int _currentCount;

        public override void StartAttack()
        {
            _currentCount = _burstCount;
            base.StartAttack();
        }

        public override void PerformAttack()
        {
            if (_currentCount <= 0)
            {
                Weapon.StartAttackCooldown(_burstCooldown);
                _currentCount = _burstCount;
                return;
            }

            ShootStrategy.Shoot();
            Weapon.Owner.PlayerAudioProvider.RPCPlayWeaponSound();

            Weapon.ChangeAmmoCount(-1);
            Weapon.Animator.AttackAnimation(AttackCooldown);
            Weapon.StartAttackCooldown(AttackCooldown);
            _currentCount--;
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