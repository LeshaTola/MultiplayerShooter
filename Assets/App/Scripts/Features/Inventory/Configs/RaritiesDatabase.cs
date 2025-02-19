using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Configs
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Rarity", fileName = "RaritiesDatabase")]
    public class RaritiesDatabase : SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<string, Rarity> Rarities { get; private set; }
        
    }
}