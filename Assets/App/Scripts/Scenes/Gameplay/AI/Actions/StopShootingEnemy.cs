using App.Scripts.Modules.AI.Actions;

namespace App.Scripts.Scenes.Gameplay.AI.Actions
{
    public class StopShootingEnemy : Action
    {
        private readonly BotAI _botAI;

        public StopShootingEnemy( BotAI botAI)
        {
            _botAI = botAI;
        }

        public override void Execute()
        {
            _botAI.WeaponProvider.CurrentWeapon.CancelAttack(false);
        }
    }
}