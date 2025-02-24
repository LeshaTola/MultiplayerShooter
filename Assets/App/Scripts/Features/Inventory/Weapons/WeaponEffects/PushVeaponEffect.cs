using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
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