using System;
using System.Collections.Generic;
using App.Scripts.Modules.StateMachine.Factories.States;
using App.Scripts.Modules.StateMachine.States.General;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Modules.StateMachine
{
    public class StateMachine
    {
        private State currentState;
        private Dictionary<Type, State> states = new();
        private IStatesFactory statesFactory;

        public StateMachine(IStatesFactory statesFactory)
        {
            this.statesFactory = statesFactory;
        }

        public State CurrentState => currentState;

        public void AddState(State state)
        {
            states.Add(state.GetType(), state);
        }

        public async UniTask ChangeState<T>()
        {
            var type = typeof(T);
            await ChangeState(type);
        }

        public async UniTask ChangeState(Type type)
        {
            if (currentState != null && currentState.GetType() == type)
            {
                return;
            }

            if (!states.ContainsKey(type))
            {
                var state = statesFactory.GetState(type);
                if (state == null)
                {
                    return;
                }

                states.Add(type, state);
            }

            if (currentState != null)
            {
                await currentState.Exit();
            }

            currentState = states[type];
            await currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}