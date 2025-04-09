using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Sounds;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.PromoCodes
{
    public class PromoCodesView:MonoBehaviour
    {
        public event Action<string> OnPromoCodeApplied;
        
        [SerializeField] private TMP_InputField _promoCodeInputField;
        [SerializeField] private Button _applyPromoCodeButton;
        
        [Header("Audio")]
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField, ValueDropdown(@"GetAudioKeys")] public string BuySound { get; private set; }
        
        public List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }
        
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