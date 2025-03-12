using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class MarketView : MonoBehaviour
    {
        [SerializeField] private List<TMPLocalizer> _names;
        [SerializeField] private List<Image> _rarityImages;
        [SerializeField] private TMPLocalizer _rarityName;
        [SerializeField] private RawImage _itemImage;
        
        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private WeaponStat _weaponStatPrefab;
        
        [SerializeField] private RenderTexture _skinTexture;
        [SerializeField] private RenderTexture _weaponTexture;
        [SerializeField] private RaritiesDatabase _raritiesDatabase;

        private readonly List<WeaponStat> _weaponStats = new();
        private ILocalizationSystem _localizationSystem;
        private SelectionProvider _selectionProvider;
        private GlobalInventory _inventory;

        private WeaponModelsUIProvider _weaponModelsUIProvider;
        private PlayerModelsUIProvider _playerModelsUIProvider;

        public void Initialize(SelectionProvider selectionProvider,
            GlobalInventory inventory,
            ILocalizationSystem localizationSystem,
            WeaponModelsUIProvider weaponModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider)
        {
            _localizationSystem = localizationSystem;
            _weaponModelsUIProvider = weaponModelsUIProvider;
            _playerModelsUIProvider = playerModelsUIProvider;

            _rarityName.Initialize(localizationSystem);
            InitializeNames();
            
            _inventory = inventory;
            _selectionProvider = selectionProvider;
            
            _selectionProvider.OnWeaponSelected += OnWeaponSelected;
            _selectionProvider.OnSkinSelected += OnSkinSelected;
        }

        public void Cleanup()
        {
            Default();
            
            _rarityName.Cleanup();
            CleanupNames();
            
            _selectionProvider.OnWeaponSelected -= OnWeaponSelected;
            _selectionProvider.OnSkinSelected -= OnSkinSelected;
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


        private void OnSkinSelected(string id)
        {
            var config = _inventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(id));
            if (config)
            {
                SetupSkin(config);
            }
        }

        private void SetupSkin(SkinConfig config)
        {
            Default();
            SetupNames(config);
            SetupRarity(config);
            SetupSkinView(config);
        }

        private void OnWeaponSelected(string id)
        {
            var config = _inventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            if (config)
            {
                SetupWeapon(config);
            }
        }

        private void SetupSkinView(SkinConfig config)
        {
            _itemImage.texture = _skinTexture;
            _playerModelsUIProvider.Setup(config.Id);
        }

        private void SetupWeapon(WeaponConfig config)
        {
            Default();
            SetupNames(config);
            SetupRarity(config);
            SetupStats(config);
            SetupWeaponView(config);
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

        private void SetupWeaponView(WeaponConfig config)
        {
            _itemImage.texture = _weaponTexture;
            _weaponModelsUIProvider.SetupWeapon(config);
        }

        private void SetupRarity(ItemConfig config)
        {
            foreach (var rarityImage in _rarityImages)
            {
                rarityImage.color = _raritiesDatabase.Rarities[config.Rarity].Color;
            }
            
            _rarityName.Key = config.Rarity;
            _rarityName.Translate();
        }

        private void InitializeNames()
        {
            foreach (var itemName in _names)
            {
                itemName.Initialize(_localizationSystem);
            }
        }

        private void SetupNames(ItemConfig config)
        {
            foreach (var itemName in _names)
            {
                itemName.Key = config.name;
                itemName.Translate();
            }
        }

        private void CleanupNames()
        {
            foreach (var itemName in _names)
            {
                itemName.Cleanup();
            }
        }
    }
}