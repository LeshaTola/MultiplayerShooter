using App.Scripts.Modules.StateMachine.States.General;
using Zenject;

namespace App.Scripts.Modules.StateMachine.Factories.StateSteps
{
    public class StateStepsFactory : IStateStepsFactory
    {
        DiContainer diContainer;

        public StateStepsFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public T GetStateStep<T>() where T : StateStep
        {
            return diContainer.Resolve<T>();
        }
    }
}