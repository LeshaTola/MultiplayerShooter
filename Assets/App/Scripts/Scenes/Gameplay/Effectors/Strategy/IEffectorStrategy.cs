using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy
{
    public interface IEffectorStrategy
    {
        public void Initialize(Effector effector);
        public UniTask Apply(Player.Player player);
    }

    public abstract class EffectorStrategy: IEffectorStrategy
    {
        protected Effector Effector;
        
        public virtual void Initialize(Effector effector)
        {
            Effector = effector;
        }

        public abstract UniTask Apply(Player.Player player);
    }
}