using App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies
{
    public interface IShootingModeStrategy
    {
        public IShootStrategy ShootStrategy { get; set;}
        
        public bool IsShooting { get;}

        public void Initialize(Weapon weapon);

        public void StartAttack();

        public void PerformAttack();

        public void CancelAttack();
        public void Import(IShootingModeStrategy original);
    }
}