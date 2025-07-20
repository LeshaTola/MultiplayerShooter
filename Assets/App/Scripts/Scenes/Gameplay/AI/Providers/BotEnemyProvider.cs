using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Providers
{
    public class BotEnemyProvider : MonoBehaviour
    {
        [SerializeField] private Collider _visionCollider;

        private readonly HashSet<Health> _enemies = new();
        public IReadOnlyCollection<Health> Enemies => _enemies;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Health enemy))
            {
                return;
            }
            _enemies.Add(enemy);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Health enemy))
            {
                return;
            }

            _enemies.Remove(enemy);
        }

        public Health GetNearestEnemy()
        {
            Vector3 currentPos = transform.position;

            var enemies =  _enemies
                .Where(enemy => enemy != null)
                .OrderBy(enemy => (enemy.transform.position - currentPos).sqrMagnitude).ToList();
            
            if (enemies.Count <= 1)
            {
                return null;
            }
            return enemies[1];
        }
    }
}