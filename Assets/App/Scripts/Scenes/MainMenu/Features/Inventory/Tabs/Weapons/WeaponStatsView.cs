using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponStatsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _weaponName;
        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private WeaponStat _weaponStatPrefab;

        [SerializeField] private Transform _spawnPoint;
        
        
        private SelectionProvider _selectionProvider;
        private GlobalInventory _inventory;

        public void Initialiе(SelectionProvider selectionProvider, GlobalInventory inventory)
        {
            _inventory = inventory;
            _selectionProvider = selectionProvider;
            _selectionProvider.OnWeaponSelected += OnWeaponSelected;
        }

        public void Cleanup()
        {
            _selectionProvider.OnWeaponSelected -= OnWeaponSelected;
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

            _weaponName.text = config.name;
            foreach (var stat in config.Stats)
            {
                var newStat = Instantiate(_weaponStatPrefab, _statsContainer);
                newStat.Setup(stat.Item1, stat.Item2);
            }
            
            Instantiate(config.Prefab, _spawnPoint);
        }

        private void Default()
        {
            foreach (Transform child in _statsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in _spawnPoint)
            {
                Destroy(child.gameObject);
            }
        }
    }
}