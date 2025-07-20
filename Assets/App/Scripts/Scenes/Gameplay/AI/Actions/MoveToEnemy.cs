using App.Scripts.Modules.AI.Actions;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Actions
{
    public class MoveToEnemy : Action
    {
        private readonly BotEnemyProvider _botEnemyProvider;
        private readonly BotAI _botAI;

        public MoveToEnemy(BotEnemyProvider botEnemyProvider,BotAI botAI)
        {
            _botEnemyProvider = botEnemyProvider;
            _botAI = botAI;
        }
        
        public override void Execute()
        {
            var nearestEnemy = _botEnemyProvider.GetNearestEnemy();
            if (nearestEnemy == null )
            {
                return;
            }

            var targetpos = nearestEnemy.transform.position;
            targetpos.y -= 1;
            _botAI.Agent.stoppingDistance = 5f;
            _botAI.SetTarget(targetpos);
        }
    }
}