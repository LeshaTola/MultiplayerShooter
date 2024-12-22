using App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public class AutoShootingStrategy: IShootingModeStrategy
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
        }

        public void CancelAttack()
        {
            IsShooting = false;
        }

        public void Import(IShootingModeStrategy exportStrategy)
        {
            
        }
    }
}