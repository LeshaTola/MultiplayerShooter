using System;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.KillChat;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class DeadState : State
    {
        private LeaderBoardProvider _leaderBoardProvider;
        private PlayerProvider _playerProvider;
        private KillChatView _killChatView;

        public DeadState(LeaderBoardProvider leaderBoardProvider,
            PlayerProvider playerProvider,
            KillChatView killChatView)
        {
            _leaderBoardProvider = leaderBoardProvider;
            _playerProvider = playerProvider;
            _killChatView = killChatView;
        }

        public override async UniTask Enter()
        {
            Debug.Log("Dead");

            _leaderBoardProvider.AddDeath();
            
            UpdateKillChat();
            
            _playerProvider.HidePlayer();
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            RespawnPlayer();
        }

        private void UpdateKillChat()
        {
            var lastHitPlayer = PhotonView.Find(_playerProvider.Player.Health.LastHitPlayerId)
                .GetComponent<Player.Player>();
            _killChatView.RPCSpawnKillElement(lastHitPlayer.NickName, _playerProvider.Player.NickName);
        }

        private async void RespawnPlayer()
        {
            await StateMachine.ChangeState<RespawnState>();
        }
    }
}