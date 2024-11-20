using System.Collections.Generic;
using App.Scripts.Modules.StateMachine.Factories.States;
using App.Scripts.Modules.StateMachine.States.General;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.StateMachine
{
    public class StateMachine
    {
        private State currentState;
        private Dictionary<string, State> states = new();
        private IStatesFactory statesFactory;

        public StateMachine(IStatesFactory statesFactory)
        {
            this.statesFactory = statesFactory;
        }

        public State CurrentState => currentState;

        public void AddState(State state)
        {
            states.Add(state.Id, state);
        }

        public async UniTask ChangeState(State state)
        {
            await ChangeState(state.Id);
        }

        public async UniTask ChangeState(string id)
        {
            if (currentState != null && currentState.Id.Equals(id))
            {
                return;
            }

            if (!states.ContainsKey(id))
            {
                var state = statesFactory.GetState(id);
                if (state == null)
                {
                    return;
                }

                states.Add(id, state);
            }

            if (currentState != null)
            {
                await currentState.Exit();
            }

            currentState = states[id];
            await currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void AddStep(string stateId, IStateStep stateStep)
        {
            if (states.ContainsKey(stateId))
            {
                states[stateId]?.AddStep(stateStep);
            }
        }
    }
}