namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public interface IShootingModeStrategy
    {
        public bool IsShooting { get;}

        public void StartAttack();

        public void PerformAttack();

        public void CancelAttack();
    }
}