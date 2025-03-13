using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Chat
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [SerializeField] private float _duration;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        public void SetupText(string player, string message)
        {
            _text.text = $"{player}: {message}";
        }

        public async void Fade()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_duration));   
            _canvasGroup.DOFade(0, _fadeDuration).onComplete += () => Destroy(gameObject);
        }

        public void SetAsLastMessage()
        {
            _rectTransform.SetAsLastSibling();
        }
    }
}