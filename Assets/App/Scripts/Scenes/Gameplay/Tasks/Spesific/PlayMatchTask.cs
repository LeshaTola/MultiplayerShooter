using App.Scripts.Modules.TasksSystem.Tasks;
using App.Scripts.Scenes.Gameplay.Timer;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.Spesific
{
    public class PlayMatchTask : Task
    {
        [SerializeField] private int _matchCount = 1;

        private readonly TimerProvider _timerProvider;

        private int _progressCount;

        public PlayMatchTask(TimerProvider timerProvider)
        {
            _timerProvider = timerProvider;
        }

        public override void Start()
        {
            _timerProvider.OnTimerExpired += OnTimerExpired;
        }

        public override void Complete()
        {
            base.Complete();
            _timerProvider.OnTimerExpired -= OnTimerExpired;
        }

        public override void Import(Task original)
        {
            var concreteTask = (PlayMatchTask) original;
            _matchCount = concreteTask._matchCount;
        }

        public override ProgressPair GetProgress()
        {
            return new ProgressPair(_progressCount, _matchCount);
        }

        public override void SetProgress(ProgressPair progress)
        {
            _progressCount = progress.Progress;
            _matchCount = progress.Target;
        }

        private void OnTimerExpired()
        {
            _progressCount++;
            Progress = (float) _progressCount / _matchCount;
        }
    }
}