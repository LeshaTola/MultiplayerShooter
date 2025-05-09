using System;
using System.Threading;
using App.Scripts.Modules.PopupAndViews.Animations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.CustomUI
{
    public class Animator:SerializedMonoBehaviour
    {
        [SerializeField] private IAnimation _animation;

        private CancellationTokenSource _cts;
        
        private async void Start()
        {
            if (_animation == null)
            {
                Debug.LogError("Animation is not assigned!");
                return; // Прерываем выполнение, если _animation не задано
            }
            
            _cts = new CancellationTokenSource();
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    await _animation.PlayShowAnimation(gameObject, _cts.Token);
                    await _animation.PlayHideAnimation(gameObject, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected on cancellation
            }
        }
        
        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}