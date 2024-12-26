using App.Scripts.Features.Screens;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class RespawnView : GameScreen
    {
        [SerializeField] private TextMeshProUGUI _beforeRespawnText;
        [SerializeField] private TextMeshProUGUI _pressButtonText;
        [SerializeField] private TextMeshProUGUI _timerText;
        
        public void ShowTimerText()
        {
            _pressButtonText.gameObject.SetActive(false);
            
            _beforeRespawnText.gameObject.SetActive(true);
            _timerText.gameObject.SetActive(true);
        }

        public void ShowPressButtonText()
        {
            _pressButtonText.gameObject.SetActive(true);
            
            _beforeRespawnText.gameObject.SetActive(false);
            _timerText.gameObject.SetActive(false);
        }
        
        public void UpdateTimer(int timer)
        {
            _timerText.text = timer.ToString();
        }
    }
}