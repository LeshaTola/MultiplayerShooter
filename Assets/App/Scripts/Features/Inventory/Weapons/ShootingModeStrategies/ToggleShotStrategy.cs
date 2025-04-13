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
            
            IsShooting = true;
            PerformAttack();
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
                if (IsMissCounted)
                {
                    Weapon.ChangeAmmoCount(-1);
                }
                else
                {
                    if (!_isMiss)
                    {
                        Weapon.ChangeAmmoCount(-1);
                    }
                }
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