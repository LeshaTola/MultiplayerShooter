using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.Gameplay.Player;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Providers
{
    public class BotEnemyProvider:MonoBehaviour
    {
        [SerializeField] private Collider _visionCollider;

        private readonly HashSet<PlayerMovement> _enemies = new();
        public IReadOnlyCollection<PlayerMovement> Enemies => _enemies;
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Enemies: {_enemies.Count}");
            if (!other.TryGetComponent(out PlayerMovement enemy))
            {
                return;   
            }
            _enemies.Add(enemy);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerMovement enemy))
            {
                return;   
            }
            _enemies.Remove(enemy);
        }

        public PlayerMovement GetNearestEnemy()
        {
            Vector3 currentPos = transform.position;

            return _enemies
                .Where(enemy => enemy != null)
                .OrderBy(enemy => (enemy.transform.position - currentPos).sqrMagnitude).ToList()[0];
        }

    }
}