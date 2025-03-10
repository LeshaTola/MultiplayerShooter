using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponStatsView : MonoBehaviour
    {
        [SerializeField] private TMPLocalizer _weaponName;
        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private WeaponStat _weaponStatPrefab;

        private readonly List<WeaponStat> _weaponStats = new();
        private ILocalizationSystem _localizationSystem;
        private SelectionProvider _selectionProvider;
        private GlobalInventory _inventory;

        private WeaponModelsUIProvider _weaponModelsUIProvider;

        public void Initialize(SelectionProvider selectionProvider,
            GlobalInventory inventory,
            ILocalizationSystem localizationSystem,
            WeaponModelsUIProvider weaponModelsUIProvider)
        {
            _localizationSystem = localizationSystem;
            _weaponModelsUIProvider = weaponModelsUIProvider;

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

        private void Default()
        {
            CleanupWeaponStats();

            _weaponModelsUIProvider.Cleanup();
        }

        private void CleanupWeaponStats()
        {
            foreach (var weaponStat in _weaponStats)
            {
                weaponStat.Cleanup();
                Destroy(weaponStat.gameObject);
            }

            _weaponStats.Clear();
        }

        private void Setup(WeaponConfig config)
        {
            Default();
            SetupHeader(config);
            SetupStats(config);
            SetupWeaponView(config);
        }

        private void SetupWeaponView(WeaponConfig config)
        {
            _weaponModelsUIProvider.SetupWeapon(config);
        }

        private void SetupStats(WeaponConfig config)
        {
            foreach (var stat in config.Stats)
            {
                var newStat = Instantiate(_weaponStatPrefab, _statsContainer);
                newStat.Initialize(_localizationSystem);
                newStat.Setup(stat.Item1, stat.Item2);
                _weaponStats.Add(newStat);
            }
        }

        private void SetupHeader(WeaponConfig config)
        {
            _weaponName.Key = config.name;
            _weaponName.Translate();
        }

        private void OnWeaponSelected(string id)
        {
            var config = _inventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            if (config)
            {
                Setup(config);
            }
        }
    }
}