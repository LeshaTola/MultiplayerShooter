using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.KillChat;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class DeadState : State
    {
        private readonly LeaderBoardProvider _leaderBoardProvider;
        private readonly PlayerProvider _playerProvider;
        private readonly KillChatView _killChatView;

        public DeadState(LeaderBoardProvider leaderBoardProvider,
            PlayerProvider playerProvider,
            KillChatView killChatView)
        {
            _leaderBoardProvider = leaderBoardProvider;
            _playerProvider = playerProvider;
            _killChatView = killChatView;
        }

        public override UniTask Enter()
        {
            Debug.Log("Dead");

            _leaderBoardProvider.AddDeath();
            UpdateKillChat();

            _playerProvider.Player.AudioProvider.PlayDestroySound();
            _playerProvider.Player.PlayerMovement.Freese();
            _playerProvider.HidePlayer();
            RespawnPlayer();
            return UniTask.CompletedTask;
        }

        private void UpdateKillChat()
        {
            var lastHitPlayer = PhotonView.Find(_playerProvider.Player.Health.LastHitPlayerId)
                .GetComponent<Player.Player>();
            string weaponId = _playerProvider.Player.Health.LastHitWeaponId;
            GameAnalytics.NewDesignEvent($"game:weapon:{weaponId}", 1);

            _killChatView.RPCSpawnKillElement(weaponId, lastHitPlayer.NickName, _playerProvider.Player.NickName);
        }

        private async void RespawnPlayer()
        {
            await StateMachine.ChangeState<RespawnState>();
        }
    }
}