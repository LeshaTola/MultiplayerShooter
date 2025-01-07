using System;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Factory;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles
{
    public class ProjectilesShootingStrategy: ShootStrategy
    {
        public override event Action<Vector3, float> OnPlayerHit;
        
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float _projectileSpeed;
        
        private readonly Camera _camera;
        private readonly ProjectilesFactory _factory;

        public ProjectilesShootingStrategy(Camera camera, ProjectilesFactory factory)
        {
            _camera = camera;
            _factory = factory;
        }


        public override void Shoot()
        {
            base.Shoot();
            
            var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
            
            var targetPoint = Physics.Raycast(ray, out var hit) ? hit.point : ray.GetPoint(75);
            var directionWithoutSpread = targetPoint - Weapon.ShootPoint.position;
            
            var spread = Recoil.GetRecoil();
            var direction = directionWithoutSpread + spread;
            direction.Normalize();
            
            var projectile = _factory.CreateProjectile(projectile: _projectile);
            projectile.transform.position = Weapon.ShootPoint.position;
            projectile.transform.forward = direction.normalized;
            projectile.gameObject.SetActive(true);
            
            projectile.Setup(Weapon);
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