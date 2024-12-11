using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.KillChat
{
    public class KillElementView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killerText;
        [SerializeField] private TextMeshProUGUI _victimText;

        [Header("animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _timeBeforeFade;
        [SerializeField] private float _fadeTime;

        public async void Setup(string killer, string victim)
        {
            _killerText.text = killer;
            _victimText.text = victim;

            await UniTask.Delay(TimeSpan.FromSeconds(_timeBeforeFade));
            _canvasGroup.DOFade(0, _fadeTime).onComplete += OnCompleteAnimation;
        }

        private void OnCompleteAnimation()
        {
            Destroy(gameObject);
        }
    }
}