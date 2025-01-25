using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Screen;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class InventoryState : State
    {
        private InventoryScreenPresenter _inventoryScreenPresenter;

        public InventoryState(InventoryScreenPresenter inventoryScreenPresenter)
        {
            _inventoryScreenPresenter = inventoryScreenPresenter;
        }

        public override async UniTask Enter()
        {
            await _inventoryScreenPresenter.Show();
        }

        public override async UniTask Exit()
        {
            await _inventoryScreenPresenter.Hide();
        }
    }
}