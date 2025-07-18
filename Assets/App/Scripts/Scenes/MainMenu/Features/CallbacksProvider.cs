using System;
using App.Scripts.Features;
using App.Scripts.Features.StateMachines.States;
using App.Scripts.Modules.StateMachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features
{
    public class CallbacksProvider : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private ConnectionProvider _connectionProvider;

        [Inject]
        public void Construct(StateMachine stateMachine, ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _stateMachine = stateMachine;
        }

        private void OnEnable()
        {
            _connectionProvider.OnJoinedRoomEvent += OnJoinedRoom;
        }

        private void OnDisable()
        {
            _connectionProvider.OnJoinedRoomEvent -= OnJoinedRoom;
        }

        private void OnJoinedRoom()
        {
            _stateMachine.ChangeState<LoadSceneState>().Forget();
        }
    }
}