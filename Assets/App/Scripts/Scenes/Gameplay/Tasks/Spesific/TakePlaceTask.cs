using App.Scripts.Modules.TasksSystem.Tasks;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Timer;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.Spesific
{
    public class TakePlaceTask : Task
    {
        [SerializeField] private int _targetCount;
        [SerializeField] private int _minPlace;
        
        private readonly TimerProvider _timerProvider;
        private readonly LeaderBoardProvider _leaderBoardProvider;

        private int _progressCount;
        
        public TakePlaceTask(TimerProvider timerProvider, LeaderBoardProvider leaderBoardProvider)
        {
            _timerProvider = timerProvider;
            _leaderBoardProvider = leaderBoardProvider;
        }

        public override void Start()
        {
            base.Start();
            _timerProvider.OnTimerExpired += OnTimerExpired;
        }

        public override void Complete()
        {
            base.Complete();
            _timerProvider.OnTimerExpired -= OnTimerExpired;
        }

        public override ProgressPair GetProgress()
        {
            return new ProgressPair(_progressCount, _targetCount);
        }

        public override void SetProgress(ProgressPair progress)
        {
            _progressCount = progress.Progress;
            _targetCount = progress.Target;
        }

        public override void Import(Task original)
        {
            var concreteTask = (TakePlaceTask) original;
            _targetCount = concreteTask._targetCount;
            _minPlace = concreteTask._minPlace;
        }

        private void OnTimerExpired()
        {
            if (_leaderBoardProvider.MyPlace <= _minPlace)
            {
                UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            _progressCount++;
            Progress = (float)_progressCount/_targetCount;
        }
    }
}