using UnityEngine;

namespace App.Scripts.Modules.PopupAndViews.Views
{
    public class View : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}