using System;
using App.Scripts.Scenes.Gameplay.Weapons.ShootingModeStrategies;
using App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies;
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
            strategy.Import(original);
            
            strategy.ShootStrategy  = GetShootStrategy(original.ShootStrategy);
            return strategy;
        }

        private IShootingModeStrategy GetShootingMode(Type type)
        {
            return (IShootingModeStrategy)_container.Instantiate(type);
        }
        
        private IShootStrategy GetShootStrategy(IShootStrategy original)
        {
            var strategy = GetShootStrategy(original.GetType());
            strategy.Import(original);
            return strategy;
        }
        
        private IShootStrategy GetShootStrategy(Type type)
        {
            return (IShootStrategy)_container.Instantiate(type);
        }
    }
}