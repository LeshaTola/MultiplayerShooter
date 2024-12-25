using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class BurstShootingStrategy : ShootingMode
    {
        [SerializeField] private int _burstCount = 3;
        [SerializeField] private float _burstCooldown = 0.1f;

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
                CancelAttack();
                return;
            }

            ShootStrategy.Shoot();
            
            Weapon.ChangeAmmoCount(-1);
            Weapon.Animator.AttackAnimation(_burstCooldown);
            Weapon.StartAttackCooldown(_burstCooldown);
            _currentCount--;
        }
        
        public override void Import(IShootingModeStrategy original)
        {
            base.Import(original);
            var concreteOriginal = (BurstShootingStrategy) original;

            _burstCooldown = concreteOriginal._burstCooldown;
            _burstCount = concreteOriginal._burstCount;
        }
    }
}