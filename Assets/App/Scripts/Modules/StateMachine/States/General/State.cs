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

        public virtual UniTask Enter()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Update()
        {
            return UniTask.CompletedTask;
        }
    }
}