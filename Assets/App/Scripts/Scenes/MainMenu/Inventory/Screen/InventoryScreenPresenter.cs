using App.Scripts.Features.Screens;

namespace App.Scripts.Scenes.MainMenu.Inventory.Tabs
{
    public class InventoryScreenPresenter : GameScreenPresenter
    {
        private readonly TabSwitcher _tabSwitcher;
        private readonly InventoryTabPresenter _inventoryTabPresenter;

        public InventoryScreenPresenter(TabSwitcher tabSwitcher,
            InventoryTabPresenter inventoryTabPresenter)
        {
            _tabSwitcher = tabSwitcher;
            _inventoryTabPresenter = inventoryTabPresenter;
        }

        public override void Initialize()
        {
            _tabSwitcher.Initialize();
            _inventoryTabPresenter.Initialize();
        }
        
    }
}