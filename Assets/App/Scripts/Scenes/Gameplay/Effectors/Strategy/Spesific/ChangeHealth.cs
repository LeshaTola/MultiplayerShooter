﻿using System;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific
{
    public class ChangeHealth : EffectorStrategy
    {
        [SerializeField] private float _changeValue;
        [SerializeField] private Health _healingObject;
        
        public override UniTask Apply(IEntity iEntity)
        {
            Health health;
            if (iEntity == null)
            {
                if (_healingObject == null)
                {
                    return UniTask.CompletedTask;
                }
                
                health = _healingObject;
            }
            else
            {
                health = iEntity.Health;
            }
            
            if (_changeValue > 0)
            {
                health.RPCTakeHeal(_changeValue);
                return UniTask.CompletedTask;
            }

            var damage = -_changeValue;
            health.RPCSetLasHit(iEntity.PhotonView.ViewID, null);
            health.RPCTakeDamage(damage);
            return UniTask.CompletedTask;
        }
    }
}