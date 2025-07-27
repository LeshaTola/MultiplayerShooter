using System.Collections.Generic;
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

            var origin = GetOrigin();
            var direction = GetDirection(origin);
            ApplyEffects(GetHits(direction, origin));
        }

        private void ApplyEffects(List<(Vector3, GameObject)> data)
        {
            foreach (var weaponEffect in WeaponEffects)
            {
                weaponEffect.Effect(data);
            }
        }

        private List<(Vector3, GameObject)> GetHits(Vector3 direction, Vector3 origin)
        {
            List<(Vector3, GameObject)> data = new();
            for (int i = 0; i < _buckshotCount; i++)
            {
                var orientation = !Weapon.CustomTarget ? Camera.transform : Weapon.Owner.PhotonView.transform;
                var recoiledDirection = Recoil.GetRecoilRotation(orientation)* direction;

                if (!Physics.Raycast(origin, recoiledDirection, out RaycastHit hit,
                        Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    continue;
                }

                data.Add((hit.point, hit.collider.gameObject));
            }

            return data;
        }

        private Vector3 GetOrigin()
        {
            var origin = Weapon.CustomOrigin != default
                ? Camera.transform.position
                : Weapon.ShootPointProvider.ShotPoint;
            return origin;
        }

        private Vector3 GetDirection(Vector3 origin)
        {
            Vector3 direction;
            if (!Weapon.CustomTarget)
            {
                direction = Camera.transform.forward;
            }
            else
            {
                var target = Weapon.CustomTarget.position;
                target.y += 0.4f;
                direction = target - origin;
            }

            return direction;
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            var concrete = (RaycastBuckshot) original;
            _buckshotCount = concrete._buckshotCount;
        }
    }
}