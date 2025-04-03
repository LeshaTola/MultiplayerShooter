using App.Scripts.Modules.TasksSystem.Tasks;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.Spesific
{
    public class KillTask : Task
    {
        [SerializeField] private int _kills;

        private int _progressCount;

        private readonly PlayerProvider _playerProvider;

        public KillTask(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public override void Start()
        {
            base.Start();
            _playerProvider.Player.WeaponProvider.OnPlayerHit += OnHit;
        }

        public override void Complete()
        {
            base.Complete();
            _playerProvider.Player.WeaponProvider.OnPlayerHit -= OnHit;
        }

        public override ProgressPair GetProgress()
        {
            return new ProgressPair(_progressCount, _kills);
        }

        public override void SetProgress(ProgressPair progress)
        {
            _progressCount = progress.Progress;
            _kills = progress.Target;
        }

        public override void Import(Task original)
        {
            var concreteTask = (KillTask) original;
            _kills = concreteTask._kills;
        }

        private void OnHit(Vector3 hitPoint, float damage, bool isKilled)
        {
            if (isKilled)
            {
                _progressCount++;
                Progress = (float)_progressCount/_kills;
            }
        }
    }
}