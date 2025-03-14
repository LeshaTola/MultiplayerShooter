using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Strategies
{
    public abstract class PromocodeAction
    {
        [field: SerializeField] public int ExecuteCount { get; private set; } = -1;

        public abstract void Execute();

        public virtual void Import(PromocodeAction original)
        {
            ExecuteCount = original.ExecuteCount;
        }
    }
}