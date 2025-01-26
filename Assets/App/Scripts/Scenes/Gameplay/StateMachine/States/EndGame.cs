using App.Scripts.Features.Rewards;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.EndGame;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Timer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class EndGame : State
    {
        private readonly EndGameViewPresenter _endGameViewPresenter;
        private readonly RewardProvider _rewardProvider;
        private readonly GameConfig _gameConfig;
        private readonly TimerProvider _timerProvider;
        private readonly Modules.Timer _timer;

        public EndGame(EndGameViewPresenter endGameViewPresenter,
            RewardProvider rewardProvider,
            GameConfig gameConfig,
            TimerProvider timerProvider)
        {
            _endGameViewPresenter = endGameViewPresenter;
            _rewardProvider = rewardProvider;
            _gameConfig = gameConfig;
            _timerProvider = timerProvider;
            _timer = new();
        }

        public override async UniTask Enter()
        {
            Debug.Log("End");
            
            _rewardProvider.ApplyEndMatchRewards();
            
            await _endGameViewPresenter.Show();
            await _timer.StartTimer(_gameConfig.EndGameTime, _endGameViewPresenter.UpdateTimer);
            await _endGameViewPresenter.Hide();
            _timerProvider.Initialize();
            await StateMachine.ChangeState<RespawnState>();
        }
    }
}