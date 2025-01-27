using System;
using System.Threading;
using System.Threading.Tasks;
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
        private CancellationTokenSource _cts;

        public GameplayState(PlayerProvider playerProvider, PostProcessingProvider postProcessingProvider,
            IUpdateService updateService)
        {
            _playerProvider = playerProvider;
            _postProcessingProvider = postProcessingProvider;
            _updateService = updateService;
        }

        public  override async UniTask Enter()
        {
            Debug.Log("Gameplay");
            _playerProvider.Player.Health.OnDied += OnPlayerDeath;
            _playerProvider.Player.Health.OnDamage += ApplyDamageEffect;
            _playerProvider.Player.Health.OnHealing += ApplyHealingEffect;
            
            await SetImmortal();
        }

        private async UniTask SetImmortal()
        {
            _playerProvider.Player.Health.RPCSetImmortal(true);
            _postProcessingProvider.ApplyImmortalEffect();
            _playerProvider.Player.PlayerVisual.RPCSetImortal(true);
            _cts = new CancellationTokenSource();
            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(_playerProvider.Player.PlayerConfig.ImmortalTime),
                    cancellationToken: _cts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
            
            
            _playerProvider.Player.Health.RPCSetImmortal(false);
            _playerProvider.Player.PlayerVisual.RPCSetImortal(false);
            _postProcessingProvider.RemoveImmortalEffect();
            
            _playerProvider.Player.Health.SetImmortal(true);
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
            _playerProvider.Player.Health.OnHealing -= ApplyHealingEffect;
            
            _cts?.Cancel();
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

        private void ApplyHealingEffect(float obj)
        {
            _postProcessingProvider.ApplyHealEffect();
        }
    }
}