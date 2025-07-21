using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.AI.Actions;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI
{
    [CreateAssetMenu(menuName = "Configs/AI/BotConfig", fileName = "BotConfig")]
    public class BotConfig : ScriptableObject
    {
       [field: SerializeField] public List<WeaponConfig> Weapons { get; private set; }
    }
}