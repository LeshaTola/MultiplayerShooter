using System;
using System.Collections.Generic;
using App.Scripts.Modules.ObjectPool.KeyPools.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.ObjectPool.KeyPools
{
    [Serializable]
    public class KeyPoolParticleSelectionModule
    {
        [SerializeField]
        private ParticlesDatabase particlesDatabase;

        [SerializeField]
        [ShowIf("@particlesDatabase != null")]
        [ValueDropdown(nameof(GetParticleKeys))]
        private List<string> particleKeys;

        public List<string> ParticleKeys => particleKeys;

        public IEnumerable<string> GetParticleKeys()
        {
            if (particlesDatabase == null)
            {
                return null;
            }

            return new List<string>(particlesDatabase.Particles.Keys);
        }
    }
}