using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Invokers;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections
{
    public class MarketSectionView : MonoBehaviour
    {
        public event Action OnUpdateButtonClicked;
        public event Action<string> OnItemClicked;
        public event Action OnCurrencyInvoked;
        

        [SerializeField] private List<PromocodeInvoker> _currencyMarketElements;
        [SerializeField] private List<ShopMarketElement> _weaponMarketElements;
        [SerializeField] private List<ShopMarketElement> _skinsMarketElements;

        [Header("Timer")]
        [SerializeField] private string _preTimerText;

        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _timerImage;
        [SerializeField] private Button _updateButton;

        [Header("Audio")]
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField, ValueDropdown(@"GetAudioKeys")] public string ClickSound { get; private set; }

        
        private ILocalizationSystem _localizationSystem;

        public void Initialzie(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _updateButton.onClick.AddListener(() => OnUpdateButtonClicked?.Invoke());

            foreach (var shopMarketElement in _weaponMarketElements)
            {
                shopMarketElement.Initialize();
            }
            foreach (var shopMarketElement in _skinsMarketElements)
            {
                shopMarketElement.Initialize();
            }

            foreach (var currencyMarketElement in _currencyMarketElements)
            {
                currencyMarketElement.OnInvoked += OnCurrencyMarketElementInvoked;
            }
        }

        public void Cleanup()
        {
            _updateButton.onClick.RemoveAllListeners();
            
            foreach (var shopMarketElement in _weaponMarketElements)
            {
                shopMarketElement.Cleanup();
            }
            foreach (var shopMarketElement in _skinsMarketElements)
            {
                shopMarketElement.Cleanup();
            }
            
            foreach (var currencyMarketElement in _currencyMarketElements)
            {
                currencyMarketElement.OnInvoked -= OnCurrencyMarketElementInvoked;
            }
        }

        public void UpdateTimer(float currentTime, float maxTime)
        {
            int hours = (int) (currentTime / 3600);
            int minutes = (int) ((currentTime % 3600) / 60);
            int seconds = (int) (currentTime % 60);

            string timeText = $"{hours:00}:{minutes:00}:{seconds:00}";
            _timerText.text = _localizationSystem.Translate(_preTimerText) + " " + timeText;
            _timerImage.fillAmount = currentTime / maxTime;
        }

        public void UpdateSections(List<ShopItemData> weaponItemConfigs,List<ShopItemData> skinsItemConfigs)
        {
            foreach (var shopMarketElement in _weaponMarketElements)
            {
                shopMarketElement.OnElementClicked -= OnElementClicked;
                shopMarketElement.gameObject.SetActive(false);
            }

            for (int i = 0; i < weaponItemConfigs.Count; i++)
            {
                _weaponMarketElements[i].gameObject.SetActive(true);
                _weaponMarketElements[i].Setup(weaponItemConfigs[i]);
                _weaponMarketElements[i].OnElementClicked += OnElementClicked;
            }
            
            foreach (var shopMarketElement in _skinsMarketElements)
            {
                shopMarketElement.OnElementClicked -= OnElementClicked;
                shopMarketElement.gameObject.SetActive(false);
            }

            for (int i = 0; i < skinsItemConfigs.Count; i++)
            {
                _skinsMarketElements[i].gameObject.SetActive(true);
                _skinsMarketElements[i].Setup(skinsItemConfigs[i]);
                _skinsMarketElements[i].OnElementClicked += OnElementClicked;
            }
        }

        private void OnElementClicked(string id)
        {
            OnItemClicked?.Invoke(id);
        }

        private void OnValidate()
        {
            _timerText.text = _preTimerText;
        }

        public List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }

        public void UpdateCurrencyCount(int obj)
        {
            foreach (var currencyMarketElement in _currencyMarketElements)
            {
                currencyMarketElement.gameObject.SetActive(false);
            }

            for (int i = 0; i < obj; i++)
            {
                _currencyMarketElements[i].gameObject.SetActive(true);
            }
        }

        private void OnCurrencyMarketElementInvoked()
        {
            OnCurrencyInvoked?.Invoke();
        }
    }
}