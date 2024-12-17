using App.Scripts.Modules.StateMachine.Factories.States;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.StateMachine.States;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Bootstrap
{
    public class StatesInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStatesFactory();
            BindStateMachine();

            Container.Bind<State>().To<InitialState>().AsSingle();
            Container.Bind<State>().To<RespawnState>().AsSingle();
            Container.Bind<State>().To<DeadState>().AsSingle();
            Container.Bind<State>().To<GameplayState>().AsSingle();
            Container.Bind<State>().To<EndGame>().AsSingle();
        }
        
        private void BindStateMachine()
        {
            Container.Bind<Modules.StateMachine.StateMachine>().AsSingle();
        }

        private void BindStatesFactory()
        {
            Container.Bind<IStatesFactory>().To<StatesFactory>().AsSingle();
        }
    }
}