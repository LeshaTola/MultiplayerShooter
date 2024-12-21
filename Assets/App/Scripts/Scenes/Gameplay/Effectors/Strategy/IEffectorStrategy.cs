using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy
{
    public interface IEffectorStrategy
    {
        public UniTask Effect(Player.Player player);
    }
}