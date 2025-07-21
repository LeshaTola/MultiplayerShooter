using App.Scripts.Modules.AI.Actions;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Actions
{
    public class ShootEnemy : Action
    {
        private readonly BotEnemyProvider _botEnemyProvider;
        private readonly BotAI _botAI;

        public ShootEnemy(BotEnemyProvider botEnemyProvider, BotAI botAI)
        {
            _botEnemyProvider = botEnemyProvider;
            _botAI = botAI;
        }

        public override void Execute()
        {
            Debug.Log("Shoot");
        }
    }
}