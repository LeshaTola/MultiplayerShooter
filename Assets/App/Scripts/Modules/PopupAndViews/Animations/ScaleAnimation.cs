using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Animations
{
    public class ScaleAnimation: Animation
    {
        public override async UniTask PlayShowAnimation(GameObject target, CancellationToken cancellationToken)
        {
            target.transform.localScale = Vector3.zero;
            Tween tween = target.transform.DOScale(1, showAnimationTime);

            await tween.SetEase(showEase).ToUniTask(cancellationToken: cancellationToken);
        }

        public override async UniTask PlayHideAnimation(GameObject target, CancellationToken cancellationToken)
        {
            target.transform.localScale = Vector3.one;
            Tween tween = target.transform.DOScale(0, hideAnimationTime);
            await tween.SetEase(hideEase).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}