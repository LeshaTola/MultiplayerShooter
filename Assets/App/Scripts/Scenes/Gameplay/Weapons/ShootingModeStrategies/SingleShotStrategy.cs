namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public class SingleShotStrategy : IShootingModeStrategy
    {
        public bool IsShooting { get; private set; }
        
        public void StartAttack()
        {
            IsShooting = true;
        }

        public void PerformAttack()
        {
            CancelAttack();
        }

        public void CancelAttack()
        {
            IsShooting = false;
        }
    }
}