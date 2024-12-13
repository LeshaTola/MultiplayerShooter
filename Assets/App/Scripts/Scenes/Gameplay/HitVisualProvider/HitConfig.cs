using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.HitVisualProvider
{
    [CreateAssetMenu(fileName = "HitConfig", menuName = "Configs/HitConfig")]
    public class HitConfig : ScriptableObject
    {
        [field: Header("Animation")]
        [field: SerializeField] 
        public float ScaleValue { get; private set; } = 0f;
        [field: SerializeField] public float ScaleAnimationTime { get; private set; } = 0.1f;

        [field: Space]
        [field: SerializeField] 
        public float FadeValue { get; private set; } = 0.5f;
        [field: SerializeField] public Color FadeColor { get; private set; } = Color.white;

        [field: Space]
        [field: SerializeField] 
        public float FadeInTime { get; private set; } = 0f;

        [field: SerializeField] public float FadeOutTime { get; private set; } = 0.2f;

        [field: Header("Pool")]
        [field: SerializeField] 
        public DamageView ViewPrefab { get; private set; }
        [field: SerializeField] public float UpValue { get; private set; } = 1f;
        [field: SerializeField] public float UpTime { get; private set; } = 0.3f;
        [field: SerializeField] public float ScaleUp { get; private set; } = 0.3f;
        [field: SerializeField] public float ScaleDown { get; private set; } = 0.3f;
    }
}