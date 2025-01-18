using UnityEngine;

namespace App.Scripts.Features.Inventory.Configs
{
    
    [CreateAssetMenu(menuName = "Configs/Inventory/Skin", fileName = "SkinConfig")]
    public class SkinConfig : ItemConfig
    {
        [field: SerializeField] public Material Material { get; private set; }
    }
}