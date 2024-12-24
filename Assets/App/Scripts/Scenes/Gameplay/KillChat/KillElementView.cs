using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.KillChat
{
    public enum KillType
    {
        We,
        Them
    }
    
    public class KillElementView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killerText;
        [SerializeField] private TextMeshProUGUI _victimText;
        
        [Header("Color")]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _meColor = Color.white;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _timeBeforeFade;
        [SerializeField] private float _fadeTime;

        public async void Setup(string killer, string victim, KillType killerType = KillType.Them, KillType victimType = KillType.Them)
        {
            _killerText.text = killer;
            _victimText.text = victim;

            _killerText.color =  killerType == KillType.Them ? _defaultColor : _meColor;
            _victimText.color =  victimType == KillType.Them ? _defaultColor : _meColor;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_timeBeforeFade));
            _canvasGroup.DOFade(0, _fadeTime).onComplete += OnCompleteAnimation;
        }

        private void OnCompleteAnimation()
        {
            Destroy(gameObject);
        }
    }
}