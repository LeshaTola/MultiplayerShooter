using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using Module.AI.Actions;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotConfig : ScriptableObject
    {
        public List<IAction> Actions { get; private set; }
        public List<WeaponConfig> Weapons { get; private set; }
    }
}