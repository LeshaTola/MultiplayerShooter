using App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public class SingleShotStrategy : IShootingModeStrategy
    {
        [field: SerializeField] public IShootStrategy ShootStrategy { get; set; }

        private Weapon _weapon;
        
        public bool IsShooting { get; private set; }
        
        public void Initialize(Weapon weapon)
        {
            ShootStrategy.Initialize(weapon);
            _weapon = weapon;
        }
        
        public void StartAttack()
        {
            IsShooting = true;
        }

        public void PerformAttack()
        {
            ShootStrategy.Shoot();
            _weapon.Animator.AttackAnimation();
            _weapon.ChangeAmmoCount(-1);
            _weapon.StartAttackCooldown(_weapon.Config.AttackCooldown);
            CancelAttack();
        }

        public void CancelAttack()
        {
            IsShooting = false;
        }
        

        public void Import(IShootingModeStrategy original)
        {
            
        }
        
        
    }


    public class ToggleShotStrategy : IShootingModeStrategy
    {
        [field: SerializeField] public IShootStrategy ShootStrategy { get; set; }

        private Weapon _weapon;
        
        public bool IsShooting { get; private set; }
        
        public void Initialize(Weapon weapon)
        {
            ShootStrategy.Initialize(weapon);
            _weapon = weapon;
        }
        
        public void StartAttack()
        {
            IsShooting = true;
            ShootStrategy.Shoot();
        }

        public void PerformAttack()
        {
        }

        public void CancelAttack()
        {
            if (!IsShooting)
            {
                return;
            }

            IsShooting = false;
            ShootStrategy.Shoot();
        }
        

        public void Import(IShootingModeStrategy original)
        {
            
        }
    }
}