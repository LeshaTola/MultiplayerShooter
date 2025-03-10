using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features._3dModelsUI
{
    public class WeaponModelsUIProvider
    {
        private Transform _container;
        private string _layerName;

        public WeaponModelsUIProvider(Transform container, string layerName)
        {
            _container = container;
            _layerName = layerName;
        }

        private GameObject _weapon;

        public void SetupWeapon(WeaponConfig weaponConfig)
        {
            Cleanup();
            SetupWeaponModel(weaponConfig);
        }

        public void Cleanup()
        {
            if (_weapon == null)
            {
                return;
            }
            
            Object.Destroy(_weapon);
            _weapon = null;
        }

        private void SetupWeaponModel(WeaponConfig weaponConfig)
        {
            _weapon = Object.Instantiate(weaponConfig.Prefab, _container).gameObject;
            SetupTransform(weaponConfig);
            SetupLayer();
        }

        private void SetupLayer()
        {
            ChangeLayerRecursively.SetLayerRecursively(_weapon.transform, _layerName);
        }

        private void SetupTransform(WeaponConfig weaponConfig)
        {
            _weapon.transform.localPosition += weaponConfig.ViewOffset;
            _weapon.transform.localRotation *= Quaternion.Euler(weaponConfig.ViewRotationOffset);
            _weapon.transform.localScale = Vector3.Scale(_weapon.transform.localScale, weaponConfig.ViewScaleMultiplier);
        }
    }
}