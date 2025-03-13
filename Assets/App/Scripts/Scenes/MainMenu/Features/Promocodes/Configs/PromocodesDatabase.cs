using System.Collections.Generic;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Scenes.MainMenu.Features.Promocodes.Strategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Promocodes
{
    [CreateAssetMenu(fileName = "PromocodesDatabase", menuName = "Databases/Shop/Promocodes")]
    public class PromocodesDatabase:SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<string, List<PromocodeAction>> Promocodes { get; private set; }
    }
}