using App.Scripts.Modules.PopupAndViews.General.Controllers;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.PopupAndViews.General.Popup
{
    public interface IPopup
    {
        public IPopupController Controller { get; }

        public UniTask Show();
        public UniTask Hide();
        public void Activate();
        public void Deactivate();
        void Initialize(IPopupController controller);
    }
}