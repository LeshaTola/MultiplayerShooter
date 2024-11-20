using App.Scripts.Modules.StateMachine.States.General;

namespace App.Scripts.Modules.StateMachine.Factories.StateSteps
{
    public interface IStateStepsFactory
    {
        T GetStateStep<T>() where T : StateStep;
    }
}