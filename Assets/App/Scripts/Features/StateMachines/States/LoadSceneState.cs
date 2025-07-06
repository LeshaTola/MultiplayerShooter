using App.Scripts.Features.GameMods.Providers;
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
            private readonly ISceneTransition _sceneTransition;
            private readonly ICleanupService _cleanupService;
            private readonly GameModProvider _gameModProvider;

            public LoadSceneState(
                ISceneTransition sceneTransition,
                ICleanupService cleanupService,
                string sceneName,
                GameModProvider gameModProvider)
            {
                _sceneTransition = sceneTransition;
                _cleanupService = cleanupService;
                _gameModProvider = gameModProvider;
            }

            public override UniTask Enter()
            {
                _cleanupService.Cleanup();
                PhotonNetwork.LoadLevel(_gameModProvider.CurrentGameMod.SceneName);
                
                /*if (_sceneTransition != null)
                {
                    await _sceneTransition.PlayOnAsync();
                }*/
                
                CleanupAnimations();
                return UniTask.CompletedTask;
                
                /*PhotonNetwork.IsMessageQueueRunning = false;

                if (_sceneTransition != null)
                {
                    await _sceneTransition.PlayOnAsync();
                }

                _cleanupService.Cleanup();
                CleanupAnimations();

                PhotonNetwork.LoadLevel(_sceneName);

                await UniTask.WaitUntil(() =>
                    PhotonNetwork.LevelLoadingProgress >= 1f &&
                    SceneManager.GetActiveScene().name == _sceneName
                );

                PhotonNetwork.IsMessageQueueRunning = true;*/
            }

            private void CleanupAnimations()
            {
                DOTween.KillAll();
            }
        }
    }