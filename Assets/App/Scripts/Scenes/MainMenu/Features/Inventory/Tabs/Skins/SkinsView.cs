using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins
{
    public class SkinsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _skinNameText;
        [SerializeField] private TMPLocalizer _rarityText;
        [SerializeField] private PlayerVisual _playerVisual;
        
        private SelectionProvider _selectionProvider;
        private GlobalInventory _inventory;

        public void Initialize(SelectionProvider selectionProvider, GlobalInventory inventory)
        {
            _inventory = inventory;
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
            _playerVisual.SetSkin(skinId);
            var config = _inventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(skinId));
            if (config)
            {
                _rarityText.Key = config.Rarity;
                _rarityText.Translate();
            }
        }
    }
}