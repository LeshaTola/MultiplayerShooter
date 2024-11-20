using TMPro;
using UnityEngine;

namespace App.Scripts.Modules.Localization.Localizers
{
    public class TMPLocalizer : MonoBehaviour, ITextLocalizer
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private ILocalizationSystem localizationSystem;
        private string key = "";

        public string Key
        {
            get => key;
            set => key = value;
        }

        public TextMeshProUGUI Text
        {
            get => text;
            set => text = value;
        }

        private void OnValidate()
        {
            gameObject.TryGetComponent(out text);
        }

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            if (this.localizationSystem != null)
            {
                return;
            }

            this.localizationSystem = localizationSystem;
            key = text.text;
            localizationSystem.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        public void Translate()
        {
            var newText = localizationSystem.Translate(key);
            text.text = newText;
        }

        private void OnLanguageChanged()
        {
            Translate();
        }

        public void Cleanup()
        {
            localizationSystem.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}