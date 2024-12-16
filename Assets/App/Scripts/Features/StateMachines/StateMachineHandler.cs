using System;
using App.Scripts.Features.StateMachines.States;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.Factories.States;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.StateMachines
{
    public class StateMachineHandler : SerializedMonoBehaviour
    {
        [SerializeField] private State _nextState; 
        private StateMachine stateMachine;
        private IStatesFactory statesFactory;

        [Inject]
        public void Construct(StateMachine stateMachine, IStatesFactory statesFactory)
        {
            this.stateMachine = stateMachine;
            this.statesFactory = statesFactory;
        }

        private async void Start()
        {
            var startState = (GlobalInitialState) 
                statesFactory.GetState(typeof(GlobalInitialState));
            startState.NextState = _nextState.GetType();
            await stateMachine.ChangeState(startState.GetType());
        }

        private void Update()
        {
            stateMachine.Update();
        }
    }
}