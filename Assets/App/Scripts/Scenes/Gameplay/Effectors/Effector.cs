using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class Effector : SerializedMonoBehaviour
    {
        [SerializeField] private List<IEffectorStrategy> _strategies;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                foreach (IEffectorStrategy strategy in _strategies)
                {
                    strategy.Apply(player);
                }
            }
        }
    }
}