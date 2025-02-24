namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class ToggleShotStrategy : ShootingMode
    {
        public override void StartAttack()
        {
            if (!Weapon.IsReady)
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
        }
    }
}