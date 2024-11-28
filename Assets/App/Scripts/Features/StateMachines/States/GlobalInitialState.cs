using System;
using App.Scripts.Modules.StateMachine.States.General;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Features.StateMachines.States
{
    public class GlobalInitialState : State
    {
        public Type NextState { get; set; }

        private static bool isValid = true;

        public override async UniTask Enter()
        {
            if (!isValid)
            {
                await StateMachine.ChangeState(NextState);
                return;
            }

            await base.Enter();
            await StateMachine.ChangeState(NextState);

            isValid = false;
        }
    }
}