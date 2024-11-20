using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.PopupAndViews.General.Providers;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace App.Scripts.Modules.PopupAndViews.General.Controllers
{
    public class PopupController : IPopupController
    {
        private IPopupProvider popupProvider;
        private Image screenBlocker;
        private List<Popup.Popup> currentPopups;

        public PopupController(
            IPopupProvider popupProvider,
            Image screenBlocker
        )
        {
            this.popupProvider = popupProvider;
            this.screenBlocker = screenBlocker;
            currentPopups = new List<Popup.Popup>();
        }

        public void AddActivePopup(Popup.Popup popup)
        {
            DeactivatePrevPopup();
            popup.Canvas.sortingLayerName = "Popup";
            popup.Canvas.sortingOrder = currentPopups.Count + 1;
            currentPopups.Add(popup);
        }

        public void RemoveActivePopup(Popup.Popup popup)
        {
            if (currentPopups.Count <= 0)
            {
                return;
            }

            if (currentPopups.Last() == popup)
            {
                ActivatePrevPopup();
            }

            currentPopups.Remove(popup);
            popupProvider.PopupPoolsDictionary[popup.GetType()].Release(popup);
        }

        public T GetPopup<T>()
            where T : Popup.Popup
        {
            var type = typeof(T);
            var popup = popupProvider.PopupPoolsDictionary[type].Get();
            popup.Initialize(this);
            return (T) popup;
        }

        public async UniTask HideLastPopup()
        {
            if (currentPopups.Count <= 0)
            {
                return;
            }

            var popup = currentPopups.Last();
            await popup.Hide();
        }

        private void DeactivatePrevPopup()
        {
            if (currentPopups.Count > 0)
            {
                currentPopups.Last().Deactivate();
                return;
            }
            screenBlocker.gameObject.SetActive(true);
        }

        private void ActivatePrevPopup()
        {
            if (currentPopups.Count > 1)
            {
                currentPopups[currentPopups.Count - 2].Activate();
                return;
            }

            screenBlocker.gameObject.SetActive(false);
        }
    }
}