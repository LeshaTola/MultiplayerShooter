using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class MainState : State
    {
        private MainScreenPresenter _mainScreenPresenter;
        
        public MainState(MainScreenPresenter mainScreenPresenter)
        {
            _mainScreenPresenter = mainScreenPresenter;
        }

        public override async UniTask Enter()
        {
            _mainScreenPresenter.Setup();
            _mainScreenPresenter.Initialize();
            await _mainScreenPresenter.Show();
        }

        public override async UniTask Exit()
        {
            _mainScreenPresenter.Cleanup();
            await _mainScreenPresenter.Hide();
        }
    }
}