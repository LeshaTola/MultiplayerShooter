using App.Scripts.Features.GameMods.Providers;
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
        private readonly RewardsProvider _rewardsProvider;
        private readonly GameModProvider _gameModProvider;
        private readonly TimerProvider _timerProvider;
        private readonly LeaderBoardProvider _leaderBoardProvider;
        private readonly Modules.Timer _timer;

        private bool _exit;

        public EndGame(EndGameViewPresenter endGameViewPresenter,
            RewardsProvider rewardsProvider,
            GameModProvider gameModProvider,
            TimerProvider timerProvider,
            LeaderBoardProvider leaderBoardProvider)
        {
            _endGameViewPresenter = endGameViewPresenter;
            _rewardsProvider = rewardsProvider;
            _gameModProvider = gameModProvider;
            _timerProvider = timerProvider;
            _leaderBoardProvider = leaderBoardProvider;
            _timer = new Modules.Timer();
        }

        public override async UniTask Enter()
        {
            Debug.Log("End");
            _exit = false;
            _rewardsProvider.ApplyEndMatchRewards();

            await _endGameViewPresenter.Show();
            await _timer
                .StartTimer(
                    _gameModProvider.CurrentGameMod.GameConfig.EndGameTime,
                    _endGameViewPresenter.UpdateTimer);
            
            if (_exit)
            {
                return;
            }

            await _endGameViewPresenter.Hide();
            _timerProvider.Initialize();
            await StateMachine.ChangeState<RespawnState>();
        }

        public override UniTask Exit()
        {
            _timer.StopTimer();
            _exit = true;
            _leaderBoardProvider.Reset();
            return base.Exit();
        }
    }
}