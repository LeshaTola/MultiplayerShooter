#if RUSTORE
using System.Collections.Generic;
using System.Linq;
using RuStore.PayClient;
using TMPro;
using UnityEngine.UI;
using YG;
using Zenject;
#endif
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms.RuStore
{
    public class RuStorePurchase: MonoBehaviour
    {
#if RUSTORE
        [SerializeField] private string _tag;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private ImageLoadYG _image;
        [SerializeField] private Button _button;
        
        [Inject]
        private PromoCodesProvider _promoCodesProvider;
        
         private void Awake()
        {
#if !UNITY_ANDROID
            enabled = false;
#endif
        }

#if UNITY_ANDROID
        private void OnEnable()
        {
            ProductId [] ids = {
                new(_tag)
            };
            RuStorePayClient.Instance.GetProducts(ids,
                onFailure: error =>
                {
                    gameObject.SetActive(false);
                    Debug.Log(error.description);
                },
                onSuccess: OnFetchProducts);

            _button.onClick.AddListener(Buy);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }


        private void OnFetchProducts(List<Product> products)
        {
            if (products.Count <= 0)
            {
                Debug.LogError($"No products count 0");
                return;
            }
            
            var product = products.FirstOrDefault(x => x.productId.value.Equals(_tag));
            if (product == null)
            {
                gameObject.SetActive(false);
                Debug.LogError($"No product found for tag: {_tag}");
                return;
            }

            if (_image != null)
            {
                _image.urlImage = product.imageUrl.value;
                _image.Load();
            }

            _costText.text = $"{product.amountLabel.value}";
            _titleText.text = product.title.value;
        }

        public void Buy()
        {
            var parameters = new ProductPurchaseParams(
                productId: new ProductId(_tag),
                appUserEmail: null,
                appUserId: null,
                developerPayload: null,
                orderId: null,
                quantity: new Quantity(1)
            );

            RuStorePayClient.Instance.Purchase(
                parameters: parameters,
                preferredPurchaseType: PreferredPurchaseType.ONE_STEP,
                onFailure: (error) => {
                    switch (error) {
                        case RuStorePaymentException.ProductPurchaseCancelled cancelled:
                            // Handle cancelled purchase
                            break;
                        case RuStorePaymentException.ProductPurchaseException exception:
                            // Handle failed purchase
                            break;
                        default:
                            // Handle other error
                            break;
                    }
                },
                onSuccess: (result) => {
                    _promoCodesProvider.ApplyPromoCode(result.productId.value);
                });
        }
#endif
#endif
    }
}