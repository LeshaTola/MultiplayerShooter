using System;
using App.Scripts.Modules.ObjectPool.PooledObjects;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions
{
    public class Explosion : MonoBehaviour, IPoolableObject<Explosion>
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;
        
        private Action<Health, float> _onTriggerAction;
        private IPool<Explosion> _pool;
            
        public void Setup(float radius, Action<Health, float> onTriggerAction)
        {
            _onTriggerAction = onTriggerAction;
            _collider.radius = radius;
            
            var shape = _particleSystem.shape;
            shape.radius = radius;

            var main = _particleSystem.main;
            main.startSize = new ParticleSystem.MinMaxCurve(radius, radius * 2);
        }

        public async void Explode()
        {
            _particleSystem.Play();
            _audioSource.PlayOneShot(_audioSource.clip);
            _collider.enabled = true;
            await UniTask.DelayFrame(1);
            _collider.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_particleSystem.main.duration));
            Release();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Health health) || health.IsImortal)
            {
                return;
            }
            
            var distance = (other.transform.position - transform.position).magnitude;
            _onTriggerAction?.Invoke(health, distance);
        }

        public void OnGet(IPool<Explosion> pool)
        {
            _pool = pool;
        }

        public void Release()
        {
            _pool.Release(this);
        }

        public void OnRelease()
        {
            _collider.enabled = false;
        }
    }
}