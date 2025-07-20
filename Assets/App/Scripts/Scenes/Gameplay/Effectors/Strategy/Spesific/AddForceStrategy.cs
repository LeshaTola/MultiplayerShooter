using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific
{
    public class AddForceStrategy : EffectorStrategy
    {
        public List<ForceData> _forceData;
        
        public override async UniTask Apply(IEntity iEntity)
        {
            foreach (var forceData in _forceData)
            {
                iEntity.Movement.AddForce(forceData.ForceVector);
                await UniTask.Delay(TimeSpan.FromSeconds(forceData.Delay));
            }
        }
    }

    [Serializable]
    public class ForceData
    {
        public Vector3 ForceVector;
        public float Delay;
    }
}