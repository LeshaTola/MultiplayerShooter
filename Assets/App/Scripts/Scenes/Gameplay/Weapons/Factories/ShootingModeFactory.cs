using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies;
using App.Scripts.Features.Inventory.Weapons.WeaponEffects;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Weapons.Factories
{
    public class ShootingModeFactory
    {
        private DiContainer _container;

        public ShootingModeFactory(DiContainer container)
        {
            _container = container;
        }

        public T GetShootingMode<T>() where T : IShootingModeStrategy
        {
            var strategyType = typeof(T);
            var strategy = (T) GetShootingMode(strategyType);

            strategy.ShootStrategy = GetShootStrategy(strategy.ShootStrategy.GetType());
            return strategy;
        }

        public IShootingModeStrategy GetShootingMode(IShootingModeStrategy original)
        {
            Type type = original.GetType();
            
            var strategy = GetShootingMode(type);
            strategy.ShootStrategy  = GetShootStrategy(original.ShootStrategy);
            
            strategy.Import(original);
            return strategy;
        }

        private IShootingModeStrategy GetShootingMode(Type type)
        {
            return (IShootingModeStrategy)_container.Instantiate(type);
        }
        
        private IShootStrategy GetShootStrategy(IShootStrategy original)
        {
            var strategy = GetShootStrategy(original.GetType());

            strategy.WeaponEffects = GetWeaponEffects(original.WeaponEffects);
            return strategy;
        }
        
        private IShootStrategy GetShootStrategy(Type type)
        {
            return (IShootStrategy)_container.Instantiate(type);
        }

        private IWeaponEffect GetWeaponEffect(IWeaponEffect original)
        {
            var strategy = GetWeaponEffect(original.GetType());
            return strategy;
        }
        
        private IWeaponEffect GetWeaponEffect(Type type)
        {
            return (IWeaponEffect)_container.Instantiate(type);
        }

        private List<IWeaponEffect> GetWeaponEffects(List<IWeaponEffect> effects)
        {
            var newEffects = new List<IWeaponEffect>();
            foreach (var weaponEffect in effects)
            {
                newEffects.Add(GetWeaponEffect(weaponEffect));
            }
            return newEffects;
        }
    }
}