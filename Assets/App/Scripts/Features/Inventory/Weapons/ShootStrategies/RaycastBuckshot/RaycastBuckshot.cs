using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.RaycastBuckshot
{
    public class RaycastBuckshot : ShootStrategy
    {
        [SerializeField] private int _buckshotCount = 5;

        public RaycastBuckshot(Camera camera) : base(camera)
        {
        }

        public override void Shoot()
        {
            base.Shoot();
            Weapon.NextShootPoint();

            List<(Vector3, GameObject)> data = new();
            for (int i = 0; i < _buckshotCount; i++)
            {
                var recoilRotation = Recoil.GetRecoilRotation(Camera.transform);
                var direction = recoilRotation * Camera.transform.forward;

                if (!Physics.Raycast(Camera.transform.position, direction, out RaycastHit hit, 
                        Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    continue;
                }
                data.Add((hit.point, hit.collider.gameObject));
            }

            foreach (var weaponEffect in WeaponEffects)
            {
                weaponEffect.Effect(data);
            }
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            var concrete = (RaycastBuckshot) original;
            _buckshotCount = concrete._buckshotCount;
        }
    }
}