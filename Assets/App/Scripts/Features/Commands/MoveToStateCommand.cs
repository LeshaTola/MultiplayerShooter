using System;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.States.General;
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

    public class ClosePopup : LabeledCommand
    {
        private readonly PopupController _popupController;

        public ClosePopup(string label, PopupController popupController) : base(label)
        {
            _popupController = popupController;
        }

        public override async void Execute()
        {
            await _popupController.HideLastPopup();
        }
    }

    public class CustomCommand : LabeledCommand
    {
        private readonly Action _action;

        public CustomCommand(string label, Action action) : base(label)
        {
            _action = action;
        }

        public override void Execute()
        {
            _action?.Invoke();
        }
    }
}