using System;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.StateMachines.States
{
    public class GlobalInitialState : State
    {

        private ConnectionProvider _connectionProvider;
        private  bool _isValid = true;
        
        public Type NextState { get; set; }

        public override async UniTask Enter()
        {
            if (!_isValid)
            {
                await StateMachine.ChangeState(NextState);
                return;
            }
            
            Application.targetFrameRate = 60;
            _connectionProvider.OnConnected += OnConectedToServer;
            
            _isValid = false;
        }

        public override UniTask Exit()
        {
            _connectionProvider.OnConnected -= OnConectedToServer;
            return UniTask.CompletedTask;
        }

        private async void OnConectedToServer()
        {
             await StateMachine.ChangeState(NextState);
        }
    }
}