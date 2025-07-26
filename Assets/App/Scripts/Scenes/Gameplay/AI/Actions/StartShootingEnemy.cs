using App.Scripts.Modules.AI.Actions;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Actions
{
    public class StartShootingEnemy : Action
    {
        private readonly BotAI _botAI;

        public StartShootingEnemy( BotAI botAI)
        {
            _botAI = botAI;
        }

        public override void Execute()
        {
            _botAI.WeaponProvider.CurrentWeapon.StartAttack(false);
        }
    }
}