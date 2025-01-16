using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific
{
    public class Disable : IEffectorStrategy
    {
        [SerializeField] private EffectorSpawnPoint _effectSpawnPoint;
        
        public UniTask Apply(Player.Player player)
        {
            _effectSpawnPoint.Respawn();
            return UniTask.CompletedTask;
        }
    }
}