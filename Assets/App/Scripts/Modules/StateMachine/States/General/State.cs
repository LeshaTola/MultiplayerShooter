using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.StateMachine.States.General
{
    public abstract class State
    {
        protected StateMachine StateMachine;
        
        public void Initialize(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual async UniTask Enter()
        {
        }

        public virtual async UniTask Exit()
        {
        }

        public virtual async UniTask Update()
        {
        }
    }
}