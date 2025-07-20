using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.Gameplay.Effectors;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Providers
{
    public class EffectorsProvider : MonoBehaviour
    {
        [SerializeField] private Collider _visionCollider;

        private readonly Dictionary<Type, List<Effector>> _effectors = new();
        public IReadOnlyDictionary<Type, List<Effector>> Effectors => _effectors;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Effector effector))
                return;

            foreach (var strategy in effector.Strategies)
            {
                if (!_effectors.TryGetValue(strategy.GetType(), out var list))
                {
                    list = new List<Effector>();
                    _effectors[strategy.GetType()] = list;
                }

                if (!list.Contains(effector))
                {
                    list.Add(effector);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Effector effector))
                return;

            foreach (var strategy in effector.Strategies)
            {
                if (!_effectors.TryGetValue(strategy.GetType(), out var list))
                {
                    continue;
                }
                
                list.Remove(effector);
                if (list.Count == 0)
                {
                    _effectors.Remove(strategy.GetType());
                }
            }
        }

        public Effector GetNearestEffector(Type type)
        {
            Vector3 currentPos = transform.position;

            if (!_effectors.TryGetValue(type, out var effectors))
            {
                return null;
            }
            return effectors
                .Where(effector => effector != null)
                .OrderBy(effector => (effector.transform.position - currentPos).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}