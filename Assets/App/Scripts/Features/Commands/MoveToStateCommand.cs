using System;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Features.Commands.General;
using UnityEngine;

namespace App.Scripts.Features.Commands
{
    public class MoveToStateCommand : ICommand
    {
        private Type _state;
        private readonly StateMachine _stateMachine;

        public MoveToStateCommand(StateMachine stateMachine )
        {
            _stateMachine = stateMachine;
        }

        public void SetState<T>() where T:State
        {
            _state = typeof(T);
        }

        public async void Execute()
        {
            if (_state == null)
            {
                Debug.LogError("State is not setup");    
                return;
            }
            
            await _stateMachine.ChangeState(_state);
        }
    }
}