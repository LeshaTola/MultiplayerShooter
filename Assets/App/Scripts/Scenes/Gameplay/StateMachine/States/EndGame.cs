using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.States.General;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class EndGame : State
    {
        private readonly ICleanupService _cleanupService;


        public EndGame(ICleanupService cleanupService)
        {
            _cleanupService = cleanupService;
        }

        public override UniTask Enter()
        {
            Debug.Log("End");
            _cleanupService.Cleanup();
            PhotonNetwork.LeaveRoom();
            return UniTask.CompletedTask;
        }
    }
}