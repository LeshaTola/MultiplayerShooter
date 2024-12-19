using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Inventory.Screen
{
    public class InventoryScreenPresenter : GameScreenPresenter
    {
        private readonly InventoryScreeen _inventoryScreeen;
        private readonly TabSwitcher _tabSwitcher;
        private readonly GameInventoryViewPresenter _gameInventoryViewPresenter;
        private readonly List<InventoryTabPresenter> _inventoryTabPresenters;
        private readonly StateMachine _stateMachine;

        public InventoryScreenPresenter(InventoryScreeen inventoryScreeen,
            TabSwitcher tabSwitcher,
            GameInventoryViewPresenter gameInventoryViewPresenter,
            List<InventoryTabPresenter> inventoryTabPresenters,
            StateMachine stateMachine)
        {
            _inventoryScreeen = inventoryScreeen;
            _tabSwitcher = tabSwitcher;
            _gameInventoryViewPresenter = gameInventoryViewPresenter;
            _inventoryTabPresenters = inventoryTabPresenters;
            _stateMachine = stateMachine;
        }

        public override void Initialize()
        {
            _tabSwitcher.Initialize();
            foreach (var inventoryTabPresenter in _inventoryTabPresenters)
            {
                inventoryTabPresenter.Initialize();
            }
            _gameInventoryViewPresenter.Initialize();
            _inventoryScreeen.Initialize();
            _inventoryScreeen.CloseAction += OnCloseAction;
        }

        public override void Cleanup()
        {
            foreach (var inventoryTabPresenter in _inventoryTabPresenters)
            {
                inventoryTabPresenter.Cleanup();
            }
            
            _tabSwitcher.Cleanup();
            _gameInventoryViewPresenter.Cleanup();
            _inventoryScreeen.Cleanup();
            _inventoryScreeen.CloseAction -= OnCloseAction;
        }

        private void OnCloseAction()
        {
            _stateMachine?.ChangeState<MainState>();
        }

        public override async UniTask Show()
        {
            await _inventoryScreeen.Show();
        }

        public override async UniTask Hide()
        {
            await _inventoryScreeen.Hide();
        }
    }
}