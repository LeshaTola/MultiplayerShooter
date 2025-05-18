using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootEffects
{
    public class PushEffect:ShootEffect
    {
        [SerializeField] private float _verticalPushForce;
        [SerializeField] private float _horizontalPushForce;
        
        public override void Effect()
        {
            var dir = -Weapon.transform.forward;
            var force = new Vector3(dir.x*_horizontalPushForce,dir.y*_verticalPushForce,dir.z*_horizontalPushForce);
            Weapon.Owner.PlayerMovement.AddForce(force);
        }

        public override void Update()
        {
        }

        public override void Default()
        {
        }

        public override void Import(ShootEffect original)
        {
            base.Import(original);
            var concrete = (PushEffect) original;

            _verticalPushForce = concrete._verticalPushForce;
            _horizontalPushForce = concrete._horizontalPushForce;
        }
    }
}