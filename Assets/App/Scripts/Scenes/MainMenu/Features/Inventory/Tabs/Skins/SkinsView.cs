using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.MainMenu.Features._3dModelsUI;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins
{
    public class SkinsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _skinNameText;
        [SerializeField] private TMPLocalizer _rarityText;

        private SelectionProvider _selectionProvider;
        private GlobalInventory _inventory;
        private PlayerModelsUIProvider _playerModelsUIProvider;

        public void Initialize(SelectionProvider selectionProvider,
            GlobalInventory inventory,
            PlayerModelsUIProvider playerModelsUIProvider)
        {
            _inventory = inventory;
            _playerModelsUIProvider = playerModelsUIProvider;
            _selectionProvider = selectionProvider;
            
            _selectionProvider.OnSkinSelected += OnSkinSelected;
        }

        public void Cleanup()
        {
            _selectionProvider.OnSkinSelected -= OnSkinSelected;
        }

        private void OnSkinSelected(string skinId)
        {
            _skinNameText.text = skinId;
            var config = _inventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(skinId));

            if (!config)
            {
                return;
            }
            
            _rarityText.Key = config.Rarity;
            _rarityText.Translate();
            _playerModelsUIProvider.Setup(config.Id);
        }
    }
}