using System;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class Effector : SerializedMonoBehaviour
    {
        [SerializeField] private IEffectorStrategy _strategy;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                _strategy.Apply(player);
            }
        }
    }
}