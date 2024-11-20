using System.Collections.Generic;
using App.Scripts.Modules.ObjectPool.PooledObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.ObjectPool.KeyPools.Configs
{
    [CreateAssetMenu(fileName = "ParticlesDatabase", menuName = "Databases/Particles")]
    public class ParticlesDatabase : SerializedScriptableObject
    {
        [SerializeField] Dictionary<string, PoolObject<PoolableParticle>> particles;

        public Dictionary<string, PoolObject<PoolableParticle>> Particles => particles;

        public IEnumerable<string> GetKeys()
        {
            if (particles == null)
            {
                return null;
            }

            return new List<string>(particles.Keys);
        }
    }
}