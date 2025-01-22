using System;
using App.Scripts.Features.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens.MainScreen
{
    public class MainScreen : GameScreen
    {
        public event Action PlayButtonAction;
        public event Action RouletteButtonAction;

        [SerializeField] private Button _playButton;

        [Header("Roulette")]
        [SerializeField] private Button _rouletteButton;

        [SerializeField] private TextMeshProUGUI _ticketsCountText;

        public override void Initialize()
        {
            _playButton.onClick.AddListener(() => PlayButtonAction?.Invoke());
            _rouletteButton.onClick.AddListener(() => RouletteButtonAction?.Invoke());
        }

        public override void Cleanup()
        {
            _playButton.onClick.RemoveAllListeners();
            _rouletteButton.onClick.RemoveAllListeners();
        }

        public void SetTicketsCount(int ticketsCount)
        {
            _ticketsCountText.text = ticketsCount.ToString();
        }
    }
}