using System;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Factory;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles
{
    public class ProjectilesShootingStrategy: ShootStrategy
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float _projectileSpeed;


        public ProjectilesShootingStrategy(Camera camera) : base(camera)
        {
        }

        public override void Shoot()
        {
            base.Shoot();
            var shootPoint = Weapon.NextShootPoint();
            Weapon.RPCPlayMuzzleFlash(shootPoint);

            var ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
            
            var targetPoint = Physics.Raycast(ray, out var hit,Mathf.Infinity,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) ? hit.point : ray.GetPoint(75);
            var directionWithoutSpread = targetPoint - shootPoint;
            
            var spread = Recoil.GetRecoil();
            var direction = directionWithoutSpread + spread;
            direction.Normalize();
            
            var projectile = PhotonNetwork.Instantiate(_projectile.name, shootPoint, Camera.transform.rotation).GetComponent<Projectile>();
            projectile.transform.position = shootPoint;
            projectile.gameObject.SetActive(true);
            
            projectile.Setup((point, hitObject) =>
            {
                if (!hitObject)
                {
                    return;
                }
                foreach (var weaponEffect in WeaponEffects)
                {
                    weaponEffect.Effect(new ()
                    {
                        (point, hitObject)
                    });
                }
            });
            projectile.Shoot(direction, _projectileSpeed);
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            var concrete = (ProjectilesShootingStrategy) original;
            
            _projectile = concrete._projectile;
            _projectileSpeed = concrete._projectileSpeed;
        }
    }
}