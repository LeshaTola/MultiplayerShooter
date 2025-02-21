using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponStatsView : MonoBehaviour
    {
        [SerializeField] private TMPLocalizer _weaponName;
        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private WeaponStat _weaponStatPrefab;

        [SerializeField] private Transform _spawnPoint;
        
        private readonly List<WeaponStat> _weaponStats = new();
        private ILocalizationSystem _localizationSystem;
        private SelectionProvider _selectionProvider;
        private GlobalInventory _inventory;
        
        public void Initialize(SelectionProvider selectionProvider, GlobalInventory inventory, ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;

            _weaponName.Initialize(_localizationSystem);
            _inventory = inventory;
            _selectionProvider = selectionProvider;
            _selectionProvider.OnWeaponSelected += OnWeaponSelected;
        }

        public void Cleanup()
        {
            Default();
            _selectionProvider.OnWeaponSelected -= OnWeaponSelected;
            _weaponName.Cleanup();
        }

        private void OnWeaponSelected(string id)
        {
            var config = _inventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            if (config != null)
            {
                Setup(config);
            }
        }

        public void Setup(WeaponConfig config)
        {
            Default();

            _weaponName.Key = config.name;
            _weaponName.Translate();
            foreach (var stat in config.Stats)
            {
                var newStat = Instantiate(_weaponStatPrefab, _statsContainer);
                newStat.Initialize(_localizationSystem);
                newStat.Setup(stat.Item1, stat.Item2);
                _weaponStats.Add(newStat);
            }
            
            var weapon = Instantiate(config.Prefab, _spawnPoint);
            
            weapon.transform.localPosition += config.ViewOffset;
            weapon.transform.localRotation *= Quaternion.Euler(config.ViewRotationOffset);
            weapon.transform.localScale = Vector3.Scale(weapon.transform.localScale, config.ViewScaleMultiplier);
        }

        private void Default()
        {
            foreach (var weaponStat in _weaponStats)
            {
                weaponStat.Cleanup();
                Destroy(weaponStat.gameObject);
            }
            _weaponStats.Clear();

            foreach (Transform child in _spawnPoint)
            {
                Destroy(child.gameObject);
            }
        }
    }
}