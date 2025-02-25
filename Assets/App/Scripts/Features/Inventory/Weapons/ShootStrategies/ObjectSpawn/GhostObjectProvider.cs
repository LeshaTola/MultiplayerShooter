using System;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn
{
    using UnityEngine;

    namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.ObjectSpawn
    {
        public class GhostObjectVisualizator : MonoBehaviour
        {
            [SerializeField] private Material _ghostMaterial;
        
            private ShootStrategy _shootStrategy;
            private Projectile _prefab;
            private Projectile _ghost;

            public void Initialize(ShootStrategy shootStrategy, Projectile prefab)
            {
                _shootStrategy = shootStrategy;
                _prefab = prefab;

                _ghost = Instantiate(_prefab, transform);
                _ghost.GetComponent<Collider>().enabled = false;
                SetGhostMaterial(_ghost.gameObject);
            }

            private void LateUpdate()
            {
                var hitData = _shootStrategy.GetRaycastHit();
                var hit = hitData.Item1;

                if (hit.collider)
                {
                    _ghost.gameObject.SetActive(true);
                    _ghost.transform.position = hit.point;
                    _ghost.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
                else
                {
                    _ghost.gameObject.SetActive(false);
                }
            }

            private void SetGhostMaterial(GameObject obj)
            {
                //if (_ghostMaterial == null) return;
        
                var renderers = obj.GetComponentsInChildren<Renderer>();
                var propertyBlock = new MaterialPropertyBlock();

                foreach (var renderer in renderers)
                {
                    renderer.GetPropertyBlock(propertyBlock);
                    propertyBlock.SetColor("_BaseColor", new Color(1, 1, 1, 0.5f));
                    renderer.SetPropertyBlock(propertyBlock);
                }
            }
        }
    }

}