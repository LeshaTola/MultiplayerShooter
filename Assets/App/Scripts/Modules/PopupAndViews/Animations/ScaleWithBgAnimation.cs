using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Modules.PopupAndViews.Animations
{
    public class ScaleWithBgAnimation: ScaleAnimation
    {
        [SerializeField] private Image _bgImage;
        [SerializeField] private GameObject _window;
        [SerializeField] private float _fadeValue = 0.5f;
        
        public override async UniTask PlayShowAnimation(GameObject target, CancellationToken cancellationToken)
        {
            _bgImage.color = new Color(_bgImage.color.r, _bgImage.color.g, _bgImage.color.b, 0);
            _window.transform.localScale = Vector3.zero;
        
            var bgTween = _bgImage.DOFade(_fadeValue, showAnimationTime).SetEase(showEase);
            var bgTask = bgTween.ToUniTask(cancellationToken: cancellationToken);
            var scaleTask = base.PlayShowAnimation(_window, cancellationToken);
        
            await UniTask.WhenAll(bgTask, scaleTask);
        }

        public override async UniTask PlayHideAnimation(GameObject target, CancellationToken cancellationToken)
        {
            var bgTween = _bgImage.DOFade(0f, hideAnimationTime).SetEase(hideEase);
            var bgTask = bgTween.ToUniTask(cancellationToken: cancellationToken);
            var scaleTask = base.PlayHideAnimation(_window, cancellationToken);
        
            await UniTask.WhenAll(bgTask, scaleTask);
        }
    }
}