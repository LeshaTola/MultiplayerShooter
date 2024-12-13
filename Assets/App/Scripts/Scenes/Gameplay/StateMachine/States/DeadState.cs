using System;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.KillChat;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class DeadState : State
    {
        private LeaderBoardProvider _leaderBoardProvider;
        private PlayerProvider _playerProvider;
        private KillChatView _killChatView;
        
        public override async UniTask Enter()
        {
            _leaderBoardProvider.AddDeath();

            UpdateKillChat();
            RespawnPlayer();

            await UniTask.Delay(TimeSpan.FromSeconds(3));
            StateMachine.ChangeState<RespawnState>();
        }

        private void UpdateKillChat()
        {
            var lastHitPlayer = PhotonView.Find(_playerProvider.Player.Health.LastHitPlayerId)
                .GetComponent<Player.Player>();
            _killChatView.RPCSpawnKillElement(lastHitPlayer.NickName, _playerProvider.Player.NickName);
        }

        private void RespawnPlayer()
        {
            StateMachine.ChangeState<RespawnState>();
        }
    }
}