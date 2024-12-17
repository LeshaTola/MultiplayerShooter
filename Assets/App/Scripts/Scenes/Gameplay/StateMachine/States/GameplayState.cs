using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameplayState : State
    {
        private PlayerProvider _playerProvider;
        private PostProcessingProvider _postProcessingProvider;
        private IUpdateService _updateService;

        public GameplayState(PlayerProvider playerProvider, PostProcessingProvider postProcessingProvider,
            IUpdateService updateService)
        {
            _playerProvider = playerProvider;
            _postProcessingProvider = postProcessingProvider;
            _updateService = updateService;
        }

        public override  UniTask Enter()
        {
            Debug.Log("Gameplay");
            _playerProvider.Player.Health.OnDied += OnPlayerDeath;
            _playerProvider.Player.Health.OnDamage += ApplyDamageEffect;
            return UniTask.CompletedTask;
        }

        public override UniTask Update()
        {
            _updateService.Update();
            return UniTask.CompletedTask;
        }

        public override  UniTask Exit()
        {
            _playerProvider.Player.Health.OnDied -= OnPlayerDeath;
            _playerProvider.Player.Health.OnDamage -= ApplyDamageEffect;
            return UniTask.CompletedTask;
        }

        private async void OnPlayerDeath()
        {
            await StateMachine.ChangeState<DeadState>();
        }

        private void ApplyDamageEffect(float damage)
        {
            _postProcessingProvider.ApplyDamageEffect();
        }
    }
}