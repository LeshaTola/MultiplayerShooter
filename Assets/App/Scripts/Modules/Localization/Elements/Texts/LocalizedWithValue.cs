using System;
using TMPro;
using UnityEngine;

namespace App.Scripts.Modules.Localization.Elements.Texts
{
    public class LocalizedWithValue : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private bool _isReversed;
        
        private ILocalizationSystem _localizationSystem;
        private string _localizedPart;
        private string _value;
        
        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _localizationSystem.OnLanguageChanged += OnLanguageChanged;
        }

        public void Cleanup()
        {
            _localizationSystem.OnLanguageChanged -= OnLanguageChanged;
        }

        public void Setup(string localizedPart, string value)
        {
            _localizedPart = localizedPart;
            _value = value;
            
            Localize();
        }

        private void Localize()
        {
            if (string.IsNullOrEmpty(_localizedPart))
            {
                return;
            }
            
            var localizedText = _localizationSystem.Translate(_localizedPart);
            _text.text = _isReversed ? _value + " " + localizedText : localizedText + " "  + _value;
        }

        private void OnLanguageChanged()
        {
            Localize();
        }
    }
}