using System;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Screen
{
    public class SectorView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMPLocalizer _nameText;
        [SerializeField] private TextMeshProUGUI _percentText;

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _nameText.Initialize(localizationSystem);
        }
        
        public void Setup(SectorConfig config)
        {
            _image.color = config.Color;
            _nameText.Key = config.Name;
            _nameText.Translate();
            _percentText.text = $"{Mathf.RoundToInt(config.Percent*100)}%";
        }

        public void Cleanup()
        {
            _nameText.Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();
        }
    }
}