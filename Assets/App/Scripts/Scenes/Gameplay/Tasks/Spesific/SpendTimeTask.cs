using App.Scripts.Modules.TasksSystem.Tasks;
using App.Scripts.Scenes.Gameplay.Timer;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.Spesific
{
    public class SpendTimeTask : Task
    {
        [SerializeField] private int _targetTime;
        
        private readonly TimerProvider _timerProvider;

        private int _progressTime;
        private int _timer;
        
        public SpendTimeTask(TimerProvider timerProvider)
        {
            _timerProvider = timerProvider;
        }

        public override void Start()
        {
            base.Start();
            _timerProvider.OnTimerTick += OnTimerTick;
        }

        public override void Complete()
        {
            base.Complete();
            _timerProvider.OnTimerTick -= OnTimerTick;
        }

        public override ProgressPair GetProgress()
        {
            return new ProgressPair(_progressTime, _targetTime);
        }

        public override void SetProgress(ProgressPair progress)
        {
            _progressTime = progress.Progress;
            _targetTime = progress.Target;
        }

        public override void Import(Task original)
        {
            var concreteTask = (SpendTimeTask) original;
            _targetTime = concreteTask._targetTime;
        }

        private void UpdateProgress()
        {
            _progressTime++;
            Progress = (float)_progressTime/_targetTime;
        }

        private void OnTimerTick(double remainingTime)
        {
            _timer++;
            if (_timer >= 60)
            {
                _timer = 0;
                UpdateProgress();
            }
        }
    }
}