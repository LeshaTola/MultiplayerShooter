using App.Scripts.Modules.PopupAndViews.General.Popup;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups
{
    public class MarketPopup : Popup
    {
        protected MarketPopupVm Vm;

        public virtual void Setup(MarketPopupVm vm)
        {
            Vm = vm;
        }
    }
}