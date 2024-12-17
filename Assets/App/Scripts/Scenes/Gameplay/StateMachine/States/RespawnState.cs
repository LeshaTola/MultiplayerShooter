using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RespawnState : State
    {
        private PlayerProvider _playerProvider;

        public RespawnState(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public override async UniTask Enter()
        {
            Debug.Log("Respawn");
            
            _playerProvider.RespawnPlayer();
            await StateMachine.ChangeState<GameplayState>();
        }
    }
}