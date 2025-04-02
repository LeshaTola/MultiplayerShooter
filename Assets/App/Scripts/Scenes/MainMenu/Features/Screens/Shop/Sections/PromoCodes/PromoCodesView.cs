using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.PromoCodes
{
    public class PromoCodesView:MonoBehaviour
    {
        public event Action<string> OnPromoCodeApplied;
        
        // [SerializeField] private TMP_InputField _promoCodeInputField;
        [SerializeField] private InputField _promoCodeInputField;
        [SerializeField] private Button _applyPromoCodeButton;

        public void Initialize()
        {
            _applyPromoCodeButton.onClick.AddListener(ApplyPromoCode);
        }

        public void Cleanup()
        {
            _applyPromoCodeButton.onClick.RemoveAllListeners();
        }

        private void ApplyPromoCode()
        {
            OnPromoCodeApplied?.Invoke(_promoCodeInputField.text);
        }
    }
}