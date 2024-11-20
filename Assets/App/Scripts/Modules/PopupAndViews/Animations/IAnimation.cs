using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Animations
{
    public interface IAnimation
    {
        public UniTask PlayShowAnimation(GameObject gameObject, CancellationToken token);
        public UniTask PlayHideAnimation(GameObject gameObject, CancellationToken token);
    }
}