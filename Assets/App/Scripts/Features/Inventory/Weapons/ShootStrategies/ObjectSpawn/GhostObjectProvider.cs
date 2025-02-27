using System;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using App.Scripts.Scenes.Gameplay.Player.Stats;
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
                Destroy(_ghost.GetComponent<Health>());
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
                if (_ghostMaterial == null) return;
        
                var renderers = obj.GetComponentsInChildren<Renderer>();
                var propertyBlock = new MaterialPropertyBlock();
                Color ghostColor = new Color(1, 1, 1, 0.2f); // Полупрозрачный белый

                foreach (var renderer in renderers)
                {
                    if (renderer == null) continue;

                    renderer.GetPropertyBlock(propertyBlock);
        
                    // Проверяем, какой параметр шейдер поддерживает (_BaseColor или _Color)
                    if (renderer.sharedMaterial.HasProperty("_BaseColor"))
                    {
                        propertyBlock.SetColor("_BaseColor", ghostColor);
                    }
                    else if (renderer.sharedMaterial.HasProperty("_Color"))
                    {
                        propertyBlock.SetColor("_Color", ghostColor);
                    }

                    renderer.SetPropertyBlock(propertyBlock);
                }
            }
        }
    }

}