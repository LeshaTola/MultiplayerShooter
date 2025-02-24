using App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn.App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn
{
    public class ObjectSpawnStrategy : ShootStrategy
    {
        [SerializeField] private GhostObjectVisualizator _visualizator;
        [SerializeField] private GameObject _prefab;
        
        private GhostObjectVisualizator _ghostObjectVisualizator;

        public ObjectSpawnStrategy(Camera camera) : base(camera)
        {
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
            
            var newObject = Object.Instantiate(_prefab, hit.point, Quaternion.identity);
            newObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
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