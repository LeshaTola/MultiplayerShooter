using System;
using App.Scripts.Features.Screens;
using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Roulette.Screen
{
    public class RouletteScreen : GameScreen
    {
        public Action SpinButtonPressed;

        [SerializeField] private Image _screenBlocker;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _ticketsText;

        [SerializeField] private SectorView _sectorViewPrefab;
        [SerializeField] private RectTransform _sectorViewContainer;


        public override void Initialize()
        {
            _spinButton.onClick.AddListener(()=>SpinButtonPressed?.Invoke());
        }

        public override void Cleanup()
        {
            _spinButton.onClick.RemoveAllListeners();
        }

        public void SetupTicketsCount(int ticketsCount)
        {
            _ticketsText.text = ticketsCount.ToString();
        }

        public void Setup(RouletteConfig rouletteConfig)
        {
            foreach (var sector in rouletteConfig.Sectors)
            {
                var sectorView = Instantiate(_sectorViewPrefab, _sectorViewContainer);
                sectorView.Setup(sector);
            }
        }

        public void SetBlockSreen(bool isBlocked)
        {
            _screenBlocker.gameObject.SetActive(isBlocked);
        }
    }
}