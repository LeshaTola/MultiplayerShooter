using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies;
using App.Scripts.Modules.MinMaxValue;
using App.Scripts.Scenes.Gameplay.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootEffects
{
    public class ChangeShootingSpeedEffect : ShootEffect
    {
        [SerializeField] private float _decrement;
        [SerializeField] private float _increment;
        [SerializeField] private MinMaxFloat _minMaxValue;
        
        private  float _value;
        private float _startValue;

        public override void Initialize(Weapon weapon, ShootingMode shootingMode)
        {
            base.Initialize(weapon,shootingMode);

            _startValue = shootingMode.AttackCooldown;
            _value = _startValue;
        }

        public override void Effect()
        {
            _value += _increment;
            ShootingMode.AttackCooldown = _minMaxValue.Clamp(_value);
        }

        public override void Update()
        {
            if (ShootingMode.IsShooting)
            {
                return;
            }
            
            if (_value >= _startValue)
            {
                _value = _startValue;
                ShootingMode.AttackCooldown = _minMaxValue.Clamp(_value);
                return;
            }
            _value -= _decrement * Time.deltaTime;
            ShootingMode.AttackCooldown = _minMaxValue.Clamp(_value);
        }

        public override void Default()
        {
            _value = _startValue;
            ShootingMode.AttackCooldown = _startValue;
        }

        public override void Import(ShootEffect original)
        {
            base.Import(original);
            var concrete = (ChangeShootingSpeedEffect) original;
            _decrement = concrete._decrement;
            _increment = concrete._increment;
            _minMaxValue = concrete._minMaxValue;
        }
    }
}