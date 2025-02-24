using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.RaycastBuckshot
{
    public class RaycastBuckshot : ShootStrategy
    {
        [SerializeField] private int _buckshotCount = 5;
        private readonly Camera _camera;
        
        public RaycastBuckshot(Camera camera)
        {
            _camera = camera;
        }

        public override void Shoot()
        {
            base.Shoot();
            var shootPoint = Weapon.NextShootPoint();
            Weapon.RPCPlayMuzzleFlash(shootPoint);

            List<(Vector3, GameObject)> data = new();
            for (int i = 0; i < _buckshotCount; i++)
            {
                var recoilRotation = Recoil.GetRecoilRotation(_camera.transform);
                var direction = recoilRotation * _camera.transform.forward;

                if (!Physics.Raycast(_camera.transform.position, direction, out RaycastHit hit, 
                        Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    Weapon.NetworkSetLine(shootPoint,shootPoint + _camera.transform.forward * 100);
                    Weapon.NetworkFadeOutLine();
                    return;

                }
                data.Add((hit.point, hit.collider.gameObject));
            }

            foreach (var weaponEffect in WeaponEffects)
            {
                weaponEffect.Effect(data);
            }
            
            Weapon.NetworkSetLine(shootPoint,data.Last().Item1);
            Weapon.NetworkFadeOutLine();
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            var concrete = (RaycastBuckshot) original;
            _buckshotCount = concrete._buckshotCount;
        }
    }
}