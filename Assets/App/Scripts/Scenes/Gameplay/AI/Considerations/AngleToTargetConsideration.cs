using App.Scripts.Modules.AI.Considerations;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI.Considerations
{
    public class AngleToTargetConsiderationConfig : ConsiderationConfig
    {
        [SerializeField] public float MaxAngle { get;  private set;} = 90f;
        [SerializeField] public bool IsGreaterThan { get; private set; }
    }

    public class AngleToTargetConsideration : Consideration
    {
        [SerializeField] private AngleToTargetConsiderationConfig _config;

        private readonly BotAI _botAI;

        public AngleToTargetConsideration(AngleToTargetConsiderationConfig config, BotAI botAI)
        {
            _config = config;
            _botAI = botAI;
        }

        public override ConsiderationConfig Config => _config;

        public override float GetScore()
        {
            if (_botAI.Target == null)
            {
                return 0f;
            }
            
            Vector3 toTarget = _botAI.Target.position - _botAI.transform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude < 0.001f)
                return 0f;

            float angle = Vector3.Angle(_botAI.transform.forward, toTarget);

            return _config.IsGreaterThan
                ? Mathf.Clamp01(1f-(_config.MaxAngle/angle))
                : Mathf.Clamp01(1f - (angle / _config.MaxAngle));
        }
    }
}