using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.TargetDetector
{
    [CreateAssetMenu(menuName = "Target Detection/Detection Config")]
    public class TargetDetectionConfig : ScriptableObject
    {
        [SerializeField] private float _detectionRange = 20f;
        [SerializeField] private float _detectionRadius = 0.5f;
        [SerializeField] private LayerMask _targetLayer;

        public float DetectionRange => _detectionRange;
        public float DetectionRadius => _detectionRadius;
        public LayerMask TargetLayer => _targetLayer;
    }
}