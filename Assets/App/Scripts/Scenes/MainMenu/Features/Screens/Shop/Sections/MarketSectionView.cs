using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections
{
    public class MarketSectionView : MonoBehaviour
    {
        public event Action OnUpdateButtonClicked;
        public event Action<int> OnItemClicked;
        
        [SerializeField] private List<ShopMarketElement> _shopMarketElements;
        
        [Header("Timer")]
        [SerializeField] private string _preTimerText;

        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _updateButton;

        public void Initialzie()
        {
            _updateButton.onClick.AddListener(() => OnUpdateButtonClicked?.Invoke());
        }

        public void Cleanup()
        {
            _updateButton.onClick.RemoveAllListeners();
        }
        
        public void UpdateSections(List<ShopItemData> shopItemConfigs)
        {
            foreach (var shopMarketElement in _shopMarketElements)
            {
                shopMarketElement.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < shopItemConfigs.Count; i++)
            {
                _shopMarketElements[i].gameObject.SetActive(true);
                _shopMarketElements[i].Setup(shopItemConfigs[i]);
                var id = i;
                _shopMarketElements[i].OnElementClicked += ()=> OnItemClicked?.Invoke(id);
            }
        }

        private void OnValidate()
        {
            _timerText.text = _preTimerText;
        }
    }
}