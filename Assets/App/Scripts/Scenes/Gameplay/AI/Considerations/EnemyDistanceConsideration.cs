using App.Scripts.Modules.AI.Considerations;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Considerations
{
    public class EnemyDistanceConsiderationConfig : ConsiderationConfig
    {
        [field : SerializeField] public float Distance { get; private set; }
    }
    
    public class EnemyDistanceConsideration : Consideration
    {
        [SerializeField] private EnemyDistanceConsiderationConfig _config;
        
        private readonly BotEnemyProvider _botEnemyProvider;
        
        public override ConsiderationConfig Config => _config;

        public EnemyDistanceConsideration(EnemyDistanceConsiderationConfig config, BotEnemyProvider botEnemyProvider)
        {
            _config = config;
            _botEnemyProvider = botEnemyProvider;
        }

        public override float GetScore()
        {
            var nearestEnemy = _botEnemyProvider.GetNearestEnemy();
            if (nearestEnemy == null )
            {
                return 0;
            }
            
            var distance = Vector3.Distance(_botEnemyProvider.transform.position, nearestEnemy.transform.position);
            return 1 -distance/_config.Distance;
        }
    }
}