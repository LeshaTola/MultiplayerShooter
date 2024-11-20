using App.Scripts.Modules.ObjectPool.Pools;
using UnityEngine;

namespace App.Scripts.Modules.ObjectPool.PooledObjects
{
    public class PoolableParticle : MonoBehaviour, IPoolableObject<PoolableParticle>
    {
        [SerializeField] private ParticleSystem particle;

        private IPool<PoolableParticle> pool;

        public ParticleSystem Particle => particle;
        public float SizeMultiplier { get; private set; } = 1;
        private ParticleSystem.MainModule ParticleMain => particle.main;

        private void OnParticleSystemStopped()
        {
            Release();
        }

        public void ChangeColor(Color color)
        {
            var reserveMain = ParticleMain;
            reserveMain.startColor = color;
        }

        public void Resize(float multiplier)
        {
            SizeMultiplier = multiplier;
            transform.localScale *= SizeMultiplier;
        }

        public void OnGet(IPool<PoolableParticle> pool)
        {
            this.pool = pool;
        }

        public void OnRelease()
        {
        }

        public void Release()
        {
            if (pool != null)
            {
                Resize(1 / SizeMultiplier);
                pool.Release(this);
                return;
            }

            Destroy(gameObject);
        }
    }
}