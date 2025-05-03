using System;
using System.Threading;
using App.Scripts.Modules.PopupAndViews.Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Modules.CustomUI
{
    public class CustomHighLighter : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private IAnimation _highLighteAnimation;
    
        private CancellationTokenSource _cancellationTokenSource;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        
            _highLighteAnimation.PlayShowAnimation(gameObject, _cancellationTokenSource.Token).Forget();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        
            _highLighteAnimation.PlayHideAnimation(gameObject, _cancellationTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            OnPointerExit(null);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            
            DOTween.Kill(this);
        }
    }
}