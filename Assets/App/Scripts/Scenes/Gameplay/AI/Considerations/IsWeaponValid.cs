using App.Scripts.Modules.AI.Considerations;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Considerations
{
    public class IsWeaponValid : Consideration
    {
        [SerializeField] private InverceConsiderationConfig _config = new();
        
        private readonly BotAI _botAI;
        public override ConsiderationConfig Config => _config;

        public IsWeaponValid(InverceConsiderationConfig config, BotAI botAI)
        {
            _config = config;
            _botAI = botAI;
        }
        
        public override float GetScore()
        {
            var weapon = _botAI.WeaponProvider.CurrentWeapon;
            float score = weapon && weapon.IsReady ? 1f : 0f;

            if (_config.IsInverce)
                score = 1f - score;

            return score;
        }
    }
}