using System;
using App.Scripts.Features.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.EndGame
{
    public class EndGameView:GameScreen
    {
        public event Action OnExitButtonPressed;
        
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _timerText;
        
        public override void Initialize()
        {
            _exitButton.onClick.AddListener(ExitButtonPressed);
        }

        public override void Cleanup()
        {
            _exitButton.onClick.RemoveAllListeners();
        }

        public void UpdateTimer(int time)
        {
            _timerText.text = time.ToString();
        }

        private void ExitButtonPressed()
        {
            OnExitButtonPressed?.Invoke();
        }
    }
}