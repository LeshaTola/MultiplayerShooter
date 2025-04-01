using App.Scripts.Modules.TasksSystem.Tasks;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Tasks.Spesific
{
    public class DamageTask : Task
    {
        [SerializeField] private int _damage;
        
        private readonly PlayerProvider _playerProvider;

        private int _progressCount;

        public DamageTask(PlayerProvider playerProvider)
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
            return new ProgressPair(_progressCount, _damage);
        }

        public override void SetProgress(ProgressPair progress)
        {
            _progressCount = progress.Progress;
            _damage = progress.Target;
        }

        public override void Import(Task original)
        {
            var concreteTask = (DamageTask) original;
            _damage = concreteTask._damage;
        }

        private void Connect(Player.Player player)
        {
            _playerProvider.OnPlayerCreated -= Connect;
            player.WeaponProvider.OnPlayerHit += OnHit;
        }

        private void OnHit(Vector3 hitPoint, float damage, bool isKilled)
        {
            _progressCount += (int) damage;
            Progress = (float)_progressCount/_damage;
        }
    }
}