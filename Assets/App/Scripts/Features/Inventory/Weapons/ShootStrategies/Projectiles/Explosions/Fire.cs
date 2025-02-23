using System;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions
{
    public class Fire : MonoBehaviourPun
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        
        private Action<Health> _onTriggerAction;
        private float _radius;
        private float _lifeTime;
        private float _cooldown;

        private float  _timer;
        private float _lifeTimer;
        
        private bool _isFired;

        private void Update()
        {
            if (!_isFired)
            {
                return;
            }
            
            _lifeTimer -= Time.deltaTime;
            _timer -= Time.deltaTime;

            if (_lifeTimer <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            if (_timer <= 0)
            {
                ApplyFireEffect();
                _timer = _cooldown;
            }
        }

        public void Setup(float radius, float lifeTime, float cooldown, Action<Health> onTriggerAction)
        {
            _cooldown = cooldown;
            _lifeTime = lifeTime;
            _onTriggerAction = onTriggerAction;
            photonView.RPC(nameof(NetworSetup), RpcTarget.All, radius);
        }

        [PunRPC]
        public void NetworSetup(float radius)
        {
            _radius = radius;
            var shape = _particleSystem.shape;
            shape.radius = radius/1.5f;
        }
        
        public void StartFiring()
        {
            _lifeTimer = _lifeTime;
            _isFired = true;
            _timer = 0;
        }

        private void ApplyFireEffect()
        {
            var targets 
                = Physics.OverlapSphere(
                    transform.position,
                    _radius,
                    Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Ignore);
            
            foreach (var target in targets)
            {
                if (!target.TryGetComponent(out Health health) || health.IsImortal)
                {
                    continue;
                }
                _onTriggerAction?.Invoke(health);
            }
        }
    }
}