using App.Scripts.Modules.MinMaxValue;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller.Providers
{
    [CreateAssetMenu(menuName = "Configs/MouseSensivity", fileName = "MouseSensivityConfig")]
    public class MouseSensivityConfig : ScriptableObject
    {
        
        [field:SerializeField] public MinMaxFloat MinMaxSensivity { get; private set; } = new MinMaxFloat()
        {
            Min = 5f,
            Max = 25f,
        };
        [field:SerializeField] public float MouseSensivity { get; private set; } = 20;
    }
}