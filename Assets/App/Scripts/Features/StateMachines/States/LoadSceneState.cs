using App.Scripts.Features.SceneTransitions;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.States.General;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace App.Scripts.Features.StateMachines.States
{
        public class LoadSceneState : State
        {
            private readonly string _sceneName;
            private readonly ISceneTransition _sceneTransition;
            private readonly ICleanupService _cleanupService;

            public LoadSceneState(
                ISceneTransition sceneTransition,
                ICleanupService cleanupService,
                string sceneName)
            {
                _sceneTransition = sceneTransition;
                _cleanupService = cleanupService;
                _sceneName = sceneName;
            }

            public override async UniTask Enter()
            {
                _cleanupService.Cleanup();
                PhotonNetwork.LoadLevel(_sceneName);
                
                /*if (_sceneTransition != null)
                {
                    await _sceneTransition.PlayOnAsync();
                }*/
                
                CleanupAnimations();
            }

            private void CleanupAnimations()
            {
                DOTween.KillAll();
            }
        }
    }