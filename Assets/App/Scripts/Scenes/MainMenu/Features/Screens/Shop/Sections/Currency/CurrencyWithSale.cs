using TMPro;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Currency
{
    public class CurrencyWithSale : MonoBehaviour
    {
        [SerializeField] private PurchaseYG _purchaseYg;
        
        [Header("Sale")]
        [SerializeField] private GameObject _salePanel;
        [SerializeField] private TextMeshProUGUI _prevPriceText;
        [SerializeField] private TextMeshProUGUI _curPriceText;
        [SerializeField] private TextMeshProUGUI _saleText;
        
        private string _beforeSaleId;

        public void Initialize()
        {
            _beforeSaleId = _purchaseYg.id;
            // Debug.Log($"Start id: {_beforeSaleId}");
        }

        public void SetSale(int salePercentage)
        {
            var newId = _beforeSaleId + $"_{salePercentage}";
            // Debug.Log($"TryToLoad {newId}");
            var purchaseData = YG2.PurchaseByID(newId);
            
            if (purchaseData == null)
            {
                return;
            }
            
            _purchaseYg.id = newId;
            _purchaseYg.UpdateEntries(purchaseData);
            
            _salePanel.SetActive(true);
            _saleText.text = salePercentage + "%";

            _prevPriceText.text = (int)(float.Parse(purchaseData.priceValue)*2) + " YAN";
            _curPriceText.text = purchaseData.price;
        }

        public void RemoveSale()
        {
            _salePanel.SetActive(false);
        }
        
    }
}