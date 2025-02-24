using System;
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
            private GameObject _prefab;
            private GameObject _ghost;

            public void Initialize(ShootStrategy shootStrategy, GameObject prefab)
            {
                _shootStrategy = shootStrategy;
                _prefab = prefab;

                _ghost = Instantiate(_prefab, transform);
                SetGhostMaterial(_ghost);
            }

            private void LateUpdate()
            {
                var hitData = _shootStrategy.GetRaycastHit();
                var hit = hitData.Item1;

                if (hit.collider)
                {
                    _ghost.SetActive(true);
                    _ghost.transform.position = hit.point;
                    _ghost.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
                else
                {
                    _ghost.SetActive(false);
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