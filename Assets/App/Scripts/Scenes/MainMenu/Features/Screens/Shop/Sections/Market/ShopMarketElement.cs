using System;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization.Localizers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market
{
    public class ShopMarketElement : MonoBehaviour
    {
        public event Action<int> OnElementClicked;
        
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _weaponImage;
        [SerializeField] private TMPLocalizer _weaponNameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;
        
        [SerializeField] private RaritiesDatabase _raritiesDatabase;
        
        private int _id;

        public void Initialize()
        {
            _buyButton.onClick.AddListener(() => OnElementClicked?.Invoke(_id));
        }

        public void Cleanup()
        {
            _buyButton.onClick.RemoveAllListeners();
        }
        
        public void Setup(ShopItemData shopItemData, int id)
        {
            _id = id;
            var item = shopItemData.Item;
            _backgroundImage.color = _raritiesDatabase.Rarities[item.Rarity].Color;
            _weaponImage.sprite = item.Sprite;
            _weaponNameText.Key = item.Id;
            _weaponNameText.Translate();
            _priceText.text = shopItemData.Price + "R";
        }
    }

    public class ShopItemData
    {
        public int Price;
        public ItemConfig Item;
    }
}