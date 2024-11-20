using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.StateMachine.States.General
{
    public interface IStateStep
    {
        public bool IsComplete { get; }

        public void Initialize(State parentState, StateMachine stateMachine);
        public UniTask Enter();
        public UniTask Exit();
        public UniTask Update();
    }
}