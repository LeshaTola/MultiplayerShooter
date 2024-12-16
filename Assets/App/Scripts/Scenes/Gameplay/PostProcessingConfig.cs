using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace App.Scripts.Scenes.Gameplay
{
    [CreateAssetMenu(fileName = "PostProcessingConfig", menuName = "Configs/PostProcessing")]
    public class PostProcessingConfig : ScriptableObject
    {
        [field: Header("Vignette")] 
        [field: SerializeField]
        public float FadeInTime { get; private set; } = 0.2f;

        [field: SerializeField] public float FadeOutTime { get;  private set;} = 0.5f;
        [field: SerializeField] public float FadeValue { get;  private set;} = 0.5f;
        [field: SerializeField] public ColorParameter DamageColor { get;  private set;}
        [field: SerializeField] public ColorParameter HealColor { get;  private set;}
    }
}