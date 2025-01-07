using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific
{
    public class ChangeHealth : IEffectorStrategy
    {
        [SerializeField] private float _changeValue;
        
        public UniTask Apply(Player.Player player)
        {
            if (_changeValue > 0)
            {
                player.Health.RPCTakeHeal(_changeValue);
                return UniTask.CompletedTask;
            }

            var damage = -_changeValue;
            player.Health.RPCSetLasHitPlayer(player.photonView.ViewID);
            player.Health.RPCTakeDamage(damage);
            return UniTask.CompletedTask;
        }
    }
}