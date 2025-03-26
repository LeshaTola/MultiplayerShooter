using System;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using Module.AI.Resolver;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotAI : MonoBehaviourPun
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Transform WeaponHolder { get; private set; }

        private Weapon _weapon;
        private IActionResolver _actionResolver;
        
        public void Initialize(IActionResolver actionResolver, Weapon weapon)
        {
            _actionResolver = actionResolver;
            _weapon = weapon;
            Health.OnDied += OnDied;
        }

        private void Update()
        {
            var action = _actionResolver.GetBestAction();
            action?.Execute();
        }

        private void OnDied()
        {
            PhotonNetwork.Destroy(gameObject);
            Health.OnDied -= OnDied;
        }
    }
}