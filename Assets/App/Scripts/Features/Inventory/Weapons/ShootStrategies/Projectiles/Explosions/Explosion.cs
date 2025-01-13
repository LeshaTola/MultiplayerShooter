using System;
using App.Scripts.Modules.ObjectPool.PooledObjects;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions
{
    public class Explosion : MonoBehaviourPun
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        
        private Action<Health, float> _onTriggerAction;
        private float _radius;

        public void Setup(float radius, Action<Health, float> onTriggerAction)
        {
            _radius = radius;
            _onTriggerAction = onTriggerAction;
            var shape = _particleSystem.shape;
            shape.radius = radius;

            var main = _particleSystem.main;
            main.startSize = new ParticleSystem.MinMaxCurve(radius, radius * 2);
        }

        public async void RPCExplode()
        {
            var targets = Physics.OverlapSphere(transform.position, _radius);
            foreach (var target in targets)
            {
                if (!target.TryGetComponent(out Health health) || health.IsImortal)
                {
                    continue;
                }
            
                var distance = (target.transform.position - transform.position).magnitude;
                _onTriggerAction?.Invoke(health, distance);
            }

            photonView.RPC(nameof(Explode), RpcTarget.All);
            await UniTask.WaitForSeconds(_particleSystem.main.duration);
            Destroy(gameObject);
        }
        
        [PunRPC]
        public void Explode()
        {
            _particleSystem.Play();
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }
}