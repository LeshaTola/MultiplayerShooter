﻿namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class ToggleShotStrategy : ShootingMode
    {
        public override void StartAttack()
        {
            IsShooting = true;
            ShootStrategy.Shoot();
        }

        public override void CancelAttack()
        {
            if (!IsShooting)
            {
                return;
            }

            IsShooting = false;
            ShootStrategy.Shoot();
        }
    }
}