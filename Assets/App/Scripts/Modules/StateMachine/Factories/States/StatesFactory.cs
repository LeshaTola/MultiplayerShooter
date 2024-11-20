using System.Linq;
using App.Scripts.Modules.StateMachine.States.General;
using UnityEngine;
using Zenject;

namespace App.Scripts.Modules.StateMachine.Factories.States
{
    public class StatesFactory : IStatesFactory
    {
        private DiContainer diContainer;

        public StatesFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public State GetState(string id)
        {
            var states = diContainer.ResolveAll<State>();
            var state = states.FirstOrDefault(x => x.Id.Equals(id));
            if (state != null)
            {
                state.Initialize(diContainer.Resolve<StateMachine>());
            }
            else
            {
                Debug.LogWarning($"Can't find {id} state");
            }

            return state;
        }
    }
}