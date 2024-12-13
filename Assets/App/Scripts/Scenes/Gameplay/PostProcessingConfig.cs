using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace App.Scripts.Scenes.Gameplay
{
    [CreateAssetMenu(fileName = "PostProcessingConfig", menuName = "Configs/PostProcessing")]
    public class PostProcessingConfig : ScriptableObject
    {
        [field: Header("Vignette")] 
        [field: SerializeField]
        public float FadeInTime { get; } = 0.2f;

        [field: SerializeField] public float FadeOutTime { get; } = 0.5f;
        [field: SerializeField] public float FadeValue { get; } = 0.5f;
        [field: SerializeField] public ColorParameter DamageColor { get; }
        [field: SerializeField] public ColorParameter HealColor { get; }
    }
}