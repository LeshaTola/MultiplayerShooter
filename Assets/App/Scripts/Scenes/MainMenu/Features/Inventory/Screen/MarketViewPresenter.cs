using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Screen
{
    public class MarketViewPresenter
    {
        private readonly ILocalizationSystem _localizationSystem;
        private readonly WeaponModelsUIProvider _weaponModelsUIProvider;
        private readonly PlayerModelsUIProvider _playerModelsUIProvider;

        private readonly MarketView _marketViewView;
        private readonly InventoryProvider _inventoryProvider;
        private readonly SelectionProvider _selectionProvider;

        public MarketViewPresenter(ILocalizationSystem localizationSystem,
            WeaponModelsUIProvider weaponModelsUIProvider,
            PlayerModelsUIProvider playerModelsUIProvider,
            InventoryProvider inventoryProvider,
            MarketView marketViewView, 
            SelectionProvider selectionProvider)
        {
            _localizationSystem = localizationSystem;
            _weaponModelsUIProvider = weaponModelsUIProvider;
            _playerModelsUIProvider = playerModelsUIProvider;
            _inventoryProvider = inventoryProvider;
            _marketViewView = marketViewView;
            _selectionProvider = selectionProvider;
        }

        public void Initialize()
        {
            _marketViewView.Initialize(_localizationSystem, _weaponModelsUIProvider, _playerModelsUIProvider);
            
            _selectionProvider.OnWeaponSelected += OnWeaponSelected;
            _selectionProvider.OnSkinSelected += OnSkinSelected;
        }

        public void Cleanup()
        {
            _marketViewView.Cleanup();

            _selectionProvider.OnWeaponSelected -= OnWeaponSelected;
            _selectionProvider.OnSkinSelected -= OnSkinSelected;
        }

        public void OnSkinSelected(string id)
        {
            var config = _inventoryProvider.GlobalInventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(id));
            SelectSkin(config);
        }

        private void SelectSkin(SkinConfig config)
        {
            if (config)
            {
                _marketViewView.SetupSkin(config);
            }
        }

        public void OnWeaponSelected(string id)
        {
            var config = _inventoryProvider.GlobalInventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            OnWeaponSelected(config);
        }
        
        public void OnWeaponSelected(WeaponConfig config)
        {
            if (config)
            {
                _marketViewView.SetupWeapon(config);
            }
        }
    }
}