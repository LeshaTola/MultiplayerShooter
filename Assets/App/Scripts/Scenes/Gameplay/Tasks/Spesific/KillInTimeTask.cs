using App.Scripts.Modules.TasksSystem.Tasks;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.Spesific
{
    public class KillInTimeTask : Task
    {
        [SerializeField] private int _kills;
        [SerializeField] private int _seconds;
        [SerializeField] private int _target;

        private int _progressCount;
        private int _progressKills;

        private readonly PlayerProvider _playerProvider;

        public KillInTimeTask(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public override void Start()
        {
            base.Start();
            _playerProvider.OnPlayerCreated += Connect;
        }

        public override void Complete()
        {
            base.Complete();
            _playerProvider.Player.WeaponProvider.OnPlayerHit -= OnHit;
        }

        public override ProgressPair GetProgress()
        {
            return new ProgressPair(_progressCount, _target);
        }

        public override void SetProgress(ProgressPair progress)
        {
            _progressCount = progress.Progress;
            _target = progress.Target;
        }

        public override void Import(Task original)
        {
            var concreteTask = (KillInTimeTask) original;
            _target = concreteTask._target;
            _kills = concreteTask._kills;
            _seconds = concreteTask._seconds;
        }

        private void Connect(Player.Player player)
        {
            _playerProvider.OnPlayerCreated -= Connect;
            player.WeaponProvider.OnPlayerHit += OnHit;
        }

        private void UpdateProgress()
        {
            _progressCount++;
            Progress = (float)_progressCount/_target;
        }

        private void OnHit(Vector3 hitPoint, float damage, bool isKilled)
        {
            if (!isKilled)
            {
                return;
            }

            StartTimer().Forget();
            _progressKills++;
            if (_progressKills >= _kills)
            {
                _progressKills = 0;
                UpdateProgress();
            }
        }

        private async UniTask StartTimer()
        {
            float remainingTime = _seconds;

            while (remainingTime > 0)
            {
                await UniTask.DelayFrame(1);
                remainingTime -= Time.deltaTime;
            }

            _progressKills--;
            _progressKills = Mathf.Clamp(_progressKills, 0, _kills);
        }
    }
}