using App.Scripts.Modules.AI.Considerations;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Considerations
{
    public class HealthConsideration : Consideration
    {
       [SerializeField] private ConsiderationConfig _config = new();
        private readonly BotAI _botAI;
        public override ConsiderationConfig Config => _config;

        public HealthConsideration(ConsiderationConfig config, BotAI botAI)
        {
            _config = config;
            _botAI = botAI;
        }

        public override float GetScore()
        {
            return 1 - _botAI.Health.Value/_botAI.Health.MaxValue;
        }
    }
}