using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class EffectorTrigger:MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Effector _effector;

        private void Start()
        {
            _health.OnDied += OnDied;
        }

        private void OnDied()
        {
            _effector.ApplyEffect(null);
        }
    }
}