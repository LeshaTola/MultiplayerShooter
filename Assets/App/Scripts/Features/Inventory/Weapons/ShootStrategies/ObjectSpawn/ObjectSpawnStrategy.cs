using App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn.App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Factory;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn
{
    public class ObjectSpawnStrategy : ShootStrategy
    {
        [SerializeField] private GhostObjectVisualizator _visualizator;
        [SerializeField] private Projectile _prefab;
        
        private GhostObjectVisualizator _ghostObjectVisualizator;
        private readonly ProjectilesFactory _projectilesFactory;

        public ObjectSpawnStrategy(Camera camera, ProjectilesFactory projectilesFactory) : base(camera)
        {
            _projectilesFactory = projectilesFactory;
        }

        public override void Initialize(Weapon weapon)
        {
            base.Initialize(weapon);
            if (!_ghostObjectVisualizator)
            {
                _ghostObjectVisualizator  = Object.Instantiate(_visualizator, Weapon.transform);
                _ghostObjectVisualizator.Initialize(this, _prefab);
            }
        }

        public override void Shoot()
        {
            var hit = GetRaycastHit().Item1;
            if (!hit.collider)
            {
                return;
            }
            base.Shoot();

            var projectile =_projectilesFactory.CreateProjectile(_prefab,hit.point,Quaternion.FromToRotation(Vector3.up, hit.normal));
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
            projectile.Shoot(Vector3.zero, 0);
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            var conrete = (ObjectSpawnStrategy)original;
            _visualizator = conrete._visualizator;
            _prefab = conrete._prefab;
        }
    }
}