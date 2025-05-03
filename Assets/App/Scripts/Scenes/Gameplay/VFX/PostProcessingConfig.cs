using UnityEngine;

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
        [field: SerializeField] public Color DamageColor { get; private set; } = Color.red;
        [field: SerializeField] public Color HealColor { get; private set; } = Color.green;
        [field: SerializeField] public Color ImmortalColor { get;  private set;} = Color.blue;
    }
}