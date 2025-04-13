using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class ToggleShotStrategy : ShootingMode
    {
        [SerializeField] private bool _isCanChangeBulletCount = false;

        public override void StartAttack()
        {
            if (!Weapon.IsReady)
            {
                return;
            }
            
            PerformAttack();

            if (!IsMissCounted && _isMiss)
            {
                return;
            }
            
            IsShooting = true;
            ShootStrategy.Shoot();
        }

        public override void CancelAttack()
        {
            GuaranteedCancelAttack();
        }

        public override void GuaranteedCancelAttack()
        {
            if (!IsShooting)
            {
                return;
            }
            IsShooting = false;
            ShootStrategy.Shoot();

            ToggleChangeAmmo();
        }

        private void ToggleChangeAmmo()
        {
            if (_isCanChangeBulletCount)
            {
                Weapon.ChangeAmmoCount(-1);
            }
        }

        public override void Import(IShootingModeStrategy original)
        {
            base.Import(original);
            var concrete = (ToggleShotStrategy)original;
            _isCanChangeBulletCount = concrete._isCanChangeBulletCount;
        }
    }
}