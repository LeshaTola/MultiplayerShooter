using App.Scripts.Modules.AI.Actions;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Actions
{
        public class AimEnemy : Action
        {
            private readonly BotEnemyProvider _botEnemyProvider;
            private readonly BotAI _botAI;

            public AimEnemy(BotEnemyProvider botEnemyProvider, BotAI botAI)
            {
                _botEnemyProvider = botEnemyProvider;
                _botAI = botAI;
            }

            public override void Execute()
            {
                var enemy = _botEnemyProvider.GetNearestEnemy();
                if (enemy == null)
                    return;

                _botAI.Agent.SetDestination(_botAI.transform.position);
                _botAI.Agent.stoppingDistance = 0f;

                Vector3 direction = enemy.transform.position - _botAI.transform.position;
                direction.y = 0f; 

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    _botAI.transform.rotation = Quaternion.RotateTowards(
                        _botAI.transform.rotation,
                        targetRotation,
                        400 * Time.deltaTime
                    );
                }
            }
        }
}