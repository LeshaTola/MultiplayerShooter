using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific
{
    public class Disable : EffectorStrategy
    {
        [SerializeField] private EffectorSpawnPoint _effectSpawnPoint;
        
        public override UniTask Apply(IEntity iEntity)
        {
            _effectSpawnPoint.Respawn();
            return UniTask.CompletedTask;
        }
    }
}