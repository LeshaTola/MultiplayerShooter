using System.Threading;
using App.Scripts.Modules.MinMaxValue;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Animations
{
    public class ScaleAnimationWithStartValue: Animation
    {
        [SerializeField] private MinMaxFloat _value;
            
        public override async UniTask PlayShowAnimation(GameObject target, CancellationToken cancellationToken)
        {
            target.transform.localScale = Vector3.one * _value.Min;
            Tween tween = target.transform.DOScale(_value.Max, showAnimationTime);

            await tween.SetEase(showEase).ToUniTask(cancellationToken: cancellationToken);
        }

        public override async UniTask PlayHideAnimation(GameObject target, CancellationToken cancellationToken)
        {
            Tween tween = target.transform.DOScale(_value.Min, hideAnimationTime);
            await tween.SetEase(hideEase).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}