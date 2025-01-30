using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class ShopState : State
    {
        private readonly ShopScreenPrezenter _shopScreenPresenter;

        public ShopState(ShopScreenPrezenter shopScreenPresenter)
        {
            _shopScreenPresenter = shopScreenPresenter;
        }

        public override async UniTask Enter()
        {
            await _shopScreenPresenter.Show();
        }

        public override async UniTask Exit()
        {
            await _shopScreenPresenter.Hide();
        }

        public override UniTask Update()
        {
            _shopScreenPresenter.UpdateScrollPosition();
            return UniTask.CompletedTask;
        }
    }
}