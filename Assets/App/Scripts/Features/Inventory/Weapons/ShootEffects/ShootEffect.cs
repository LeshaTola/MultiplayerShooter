using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using Sirenix.OdinInspector;

namespace App.Scripts.Features.Inventory.Weapons.ShootEffects
{
    public abstract class ShootEffect
    {
        protected Weapon Weapon;

        protected ShootingMode ShootingMode;

        public virtual void Initialize(Weapon weapon, ShootingMode shootingMode)
        {
            Weapon = weapon;
            ShootingMode = shootingMode;
        }

        public abstract void Effect();

        public abstract void Update();

        public abstract void Default();
        
        public virtual void Import(ShootEffect original)
        {
        }
    }
}