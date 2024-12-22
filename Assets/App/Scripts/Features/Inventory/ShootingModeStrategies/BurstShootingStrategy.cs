using App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public class BurstShootingStrategy : IShootingModeStrategy
    {
        [SerializeField] private int _burstCount = 3;
        [SerializeField] private float _burstCooldown = 0.1f;

        [field: SerializeField] public IShootStrategy ShootStrategy { get; set; }

        private int _currentCount;
        private Weapon _weapon;

        public bool IsShooting { get; private set; }

        public void Initialize(Weapon weapon)
        {
            ShootStrategy.Initialize(weapon);
            _weapon = weapon;
        }

        public void StartAttack()
        {
            _currentCount = _burstCount;
            IsShooting = true;
        }

        public void PerformAttack()
        {
            if (_currentCount <= 0)
            {
                CancelAttack();
                return;
            }

            ShootStrategy.Shoot();
            _weapon.ChangeAmmoCount(-1);
            _weapon.Animator.AttackAnimation(_burstCooldown);
            _weapon.StartAttackCooldown(_burstCooldown);
            _currentCount--;
        }

        public void CancelAttack()
        {
            IsShooting = false;
        }
        
        public void Import(IShootingModeStrategy original)
        {
            var concreteOriginal = (BurstShootingStrategy) original;

            _burstCooldown = concreteOriginal._burstCooldown;
            _burstCount = concreteOriginal._burstCount;
        }
    }
}