using App.Scripts.Modules.AI.Actions;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy.Spesific;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Actions
{
    public class MoveToHealing : Action
    {
        private readonly EffectorsProvider _effectorsProvider;
        private readonly BotAI _botAI;

        public MoveToHealing(EffectorsProvider effectorsProvider,BotAI botAI)
        {
            _effectorsProvider = effectorsProvider;
            _botAI = botAI;
        }
        
        public override void Execute()
        {
            var nearestEffector = _effectorsProvider.GetNearestEffector(typeof(ChangeHealth));
            if (nearestEffector == null )
            {
                return;
            }

            _botAI.Agent.stoppingDistance = 1.2f;
            _botAI.SetTarget(nearestEffector.transform);
        }
    }
}