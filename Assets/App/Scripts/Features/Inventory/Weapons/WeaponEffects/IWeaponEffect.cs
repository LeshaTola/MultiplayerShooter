using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public interface IWeaponEffect
    {
        public event Action<Vector3, float, bool> OnPlayerHit;

        public void Effect(List<(Vector3, GameObject)> hitValues);
        public void Initialize(Weapon weapon);
        public void Import(IWeaponEffect original);
    }

    public abstract class WeaponEffect : IWeaponEffect
    {
        public abstract event Action<Vector3, float, bool> OnPlayerHit;

        protected Weapon Weapon;

        public abstract void Effect(List<(Vector3, GameObject)> hitValues);

        public virtual void Initialize(Weapon weapon)
        {
            Weapon = weapon;
        }

        public virtual void Import(IWeaponEffect original)
        {
        }
    }

    public class PushVeaponEffect : WeaponEffect
    {
        public override event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private float _verticalPushForce;
        [SerializeField] private float _horizontalPushForce;
        
        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            var dir = -Weapon.transform.forward;
            var force = new Vector3(dir.x*_horizontalPushForce,dir.y*_verticalPushForce,dir.z*_horizontalPushForce);
            Weapon.Owner.AddForce(force);
        }

        public override void Import(IWeaponEffect original)
        {
            var concrete = (PushVeaponEffect) original;
            _verticalPushForce = concrete._verticalPushForce;
            _horizontalPushForce = concrete._horizontalPushForce;
        }
    }
}