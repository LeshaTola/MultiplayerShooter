using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public class BurstShootingStrategy : IShootingModeStrategy
    {
        [SerializeField] private int _burstCount = 3;
        
        private int _currentCount;
        
        public bool IsShooting { get; set; }
        
        public void StartAttack()
        {
            _currentCount = _burstCount;
            IsShooting = true;
        }

        public void PerformAttack()
        {
            if (_currentCount <= 1)
            {
                CancelAttack();
                return;
            }

            _currentCount--;
        }

        public void CancelAttack()
        {
            IsShooting = false;
        }
    }
}