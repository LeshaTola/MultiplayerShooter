﻿using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific
{
    public class PlaySoundStrategy:EffectorStrategy
    {
        public override UniTask Apply(IEntity iEntity)
        {
            Effector.RPCPlaySoud();
            return UniTask.CompletedTask;
        }
    }
}