using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.StateMachine.States.General
{
    public abstract class StateStep : IStateStep
    {
        protected State ParentState;
        protected StateMachine StateMachine;

        public void Initialize(State parentState, StateMachine stateMachine)
        {
            ParentState = parentState;
            StateMachine = stateMachine;
        }

        public bool IsComplete { get; private set; }

        public virtual UniTask Enter()
        {
            IsComplete = false;
            return UniTask.FromResult(true);
        }

        public virtual UniTask Exit()
        {
            IsComplete = true;
            return UniTask.FromResult(true);
        }

        public virtual UniTask Update()
        {
            return UniTask.FromResult(true);
        }
    }
}