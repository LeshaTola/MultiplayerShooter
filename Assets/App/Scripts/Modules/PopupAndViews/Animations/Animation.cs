using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Animations
{
    public abstract class Animation : IAnimation
    {
        [FoldoutGroup("Show")]
        [SerializeField] protected float showAnimationTime = 0.3f;
        
        [FoldoutGroup("Show")]
        [SerializeField] protected  Ease showEase = Ease.OutBack;

        [FoldoutGroup("Hide")]
        [SerializeField] protected  float hideAnimationTime= 0.3f;

        [FoldoutGroup("Hide")]
        [SerializeField] protected  Ease hideEase = Ease.InBack;


        public abstract UniTask PlayShowAnimation(GameObject gameObject, CancellationToken token);

        public abstract UniTask PlayHideAnimation(GameObject gameObject, CancellationToken token);
    }
}