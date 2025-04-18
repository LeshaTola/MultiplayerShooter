﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds;
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
        public event Action<int> OnItemClicked;

        [SerializeField] private List<ShopMarketElement> _shopMarketElements;

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

            foreach (var shopMarketElement in _shopMarketElements)
            {
                shopMarketElement.Initialize();
            }
        }

        public void Cleanup()
        {
            _updateButton.onClick.RemoveAllListeners();
            
            foreach (var shopMarketElement in _shopMarketElements)
            {
                shopMarketElement.Cleanup();
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

        public void UpdateSections(List<ShopItemData> shopItemConfigs)
        {
            foreach (var shopMarketElement in _shopMarketElements)
            {
                shopMarketElement.OnElementClicked -= OnElementClicked;
                shopMarketElement.gameObject.SetActive(false);
            }

            for (int i = 0; i < shopItemConfigs.Count; i++)
            {
                _shopMarketElements[i].gameObject.SetActive(true);
                _shopMarketElements[i].Setup(shopItemConfigs[i], i);
                _shopMarketElements[i].OnElementClicked += OnElementClicked;
            }
        }

        private void OnElementClicked(int id)
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
    }
}