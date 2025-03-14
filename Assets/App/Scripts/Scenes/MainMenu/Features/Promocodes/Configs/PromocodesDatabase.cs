using System.Collections.Generic;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Strategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes
{
    [CreateAssetMenu(fileName = "PromocodesDatabase", menuName = "Databases/Shop/Promocodes")]
    public class PromocodesDatabase:SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<string, List<PromocodeAction>> Promocodes { get; private set; }
    }
}