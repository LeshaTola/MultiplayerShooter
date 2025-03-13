using System.Threading;
using App.Scripts.Modules.PopupAndViews.Animations;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Modules.PopupAndViews.General.Popup
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class Popup : MonoBehaviour, IPopup
    {
        [FoldoutGroup("General")]
        [SerializeReference]
        IAnimation popupAnimation;

        [FoldoutGroup("General")]
        [SerializeField]
        protected GraphicRaycaster raycaster;

        [FoldoutGroup("General")]
        [SerializeField]
        protected Canvas canvas;

        public IPopupController Controller { get; private set; }
        public Canvas Canvas => canvas;
        public bool Active { get; private set; }

        private CancellationTokenSource cancellationTokenSource;

        public void Initialize(IPopupController controller)
        {
            Controller = controller;
        }

        public virtual async UniTask Hide()
        {
            Deactivate();

            Cleanup();
            cancellationTokenSource = new CancellationTokenSource();
            await popupAnimation.PlayHideAnimation(gameObject, cancellationTokenSource.Token);
            
            Controller.RemoveActivePopup(this);
            gameObject.SetActive(false);
        }

        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            Controller.AddActivePopup(this);
            
            Cleanup();
            cancellationTokenSource = new CancellationTokenSource();
            await popupAnimation.PlayShowAnimation(gameObject, cancellationTokenSource.Token);
            
            Activate();
        }

        public void Activate()
        {
            Active = true;
            raycaster.enabled = true;
        }

        public void Deactivate()
        {
            Active = false;
            raycaster.enabled = false;
        }

        private void Cleanup()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }
    }
}