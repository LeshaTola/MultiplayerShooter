namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public class AutoShootingStrategy: IShootingModeStrategy
    {
        public bool IsShooting { get; private set; }
        
        public void StartAttack()
        {
            IsShooting = true;
        }

        public void PerformAttack()
        {
        }

        public void CancelAttack()
        {
            IsShooting = false;
        }
    }
}