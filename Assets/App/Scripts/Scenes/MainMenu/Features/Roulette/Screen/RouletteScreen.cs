using System;
using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Screen
{
    public class RouletteScreen : GameScreen
    {
        public event Action SpinButtonPressed;

        [SerializeField] private Image _screenBlocker;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _ticketsText;

        [SerializeField] private SectorView _sectorViewPrefab;
        [SerializeField] private RectTransform _sectorViewContainer;

        private ILocalizationSystem _localizationSystem;
        private List<SectorView> _sectorViews;
        
        [Inject]
        public void Construct(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public override void Initialize()
        {
            _spinButton.onClick.AddListener(()=>SpinButtonPressed?.Invoke());
        }

        public override void Cleanup()
        {
            _spinButton.onClick.RemoveAllListeners();
            foreach (var sectorView in _sectorViews)
            {
                sectorView.Cleanup();
                Destroy(sectorView.gameObject);
            }
            _sectorViews.Clear();
        }

        public void SetupTicketsCount(int ticketsCount)
        {
            _ticketsText.text = ticketsCount.ToString();
        }

        public void Setup(RouletteConfig rouletteConfig)
        {
            _sectorViews = new();
            foreach (var sector in rouletteConfig.Sectors)
            {
                var sectorView = Instantiate(_sectorViewPrefab, _sectorViewContainer);
                sectorView.Initialize(_localizationSystem);
                sectorView.Setup(sector);
                _sectorViews.Add(sectorView);
            }
        }

        public void SetBlockSreen(bool isBlocked)
        {
            _screenBlocker.gameObject.SetActive(isBlocked);
        }
    }
}