using App.Scripts.Features.StateMachines.States;
using App.Scripts.Modules.StateMachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features
{
    public class CallbacksProvider : MonoBehaviourPunCallbacks
    {
        private StateMachine _stateMachine;

        [Inject]
        public void Construct(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void OnJoinedRoom()
        {
            _stateMachine.ChangeState<LoadSceneState>().Forget();
        }
    }
}