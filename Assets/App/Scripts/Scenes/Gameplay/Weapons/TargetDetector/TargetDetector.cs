using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.TargetDetector
{
    public class TargetDetector
    {
        private readonly TargetDetectionConfig _config;
        private readonly Transform _originTransform;

        public TargetDetector(TargetDetectionConfig config, Camera camera)
        {
            _config = config;
            _originTransform = camera.transform;
        }

        public bool IsTargetInSight()
        {
            Vector3 origin = _originTransform.position;
            Vector3 direction = _originTransform.forward;

            if (Cast(origin, direction, out var hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out Health health))
                {
                    Debug.Log("detected target");
                    return true;
                }
            }

            return false;
        }

        public void DrawGizmos()
        {
            if (_originTransform == null) return;

            Vector3 origin = _originTransform.position;
            Vector3 direction = _originTransform.forward;

            // Основной луч
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction * _config.DetectionRange);

            // Сфера на конце диапазона
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawWireSphere(origin + direction * _config.DetectionRange, _config.DetectionRadius);

            // Сфера в начале (для визуализации объема)
            Gizmos.DrawWireSphere(origin, _config.DetectionRadius);
        }

        private bool Cast(Vector3 origin, Vector3 direction, out RaycastHit hit)
        {
            bool isAnyObject = Physics.SphereCast(
                origin,
                _config.DetectionRadius, // ← радиус сферы
                direction,
                out hit,
                _config.DetectionRange,  // ← дальность кастования
                _config.TargetLayer
            );
            return isAnyObject;
        }
    }
}