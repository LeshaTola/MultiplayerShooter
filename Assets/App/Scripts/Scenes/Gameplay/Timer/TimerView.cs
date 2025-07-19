using System;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TimerProvider _timerProvider;

        private void OnEnable()
        {
            Debug.Log("Timer OnEnable");
            _timerProvider.OnTimerTick += OnTimerTick;
        }

        private void OnDisable()
        {
            _timerProvider.OnTimerTick -= OnTimerTick;
        }

        private void UpdateView(TimeSpan remainingTime)
        {
            _timer.text = $"{remainingTime.Minutes:00}:{remainingTime.Seconds:00}";
        }

        private void OnTimerTick(double timer)
        {
            UpdateView(TimeSpan.FromSeconds(timer));
        }
    }
}