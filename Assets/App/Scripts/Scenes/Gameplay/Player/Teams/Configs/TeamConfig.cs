using System;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Teams.Configs
{
    [Serializable]
    public class TeamConfig : ScriptableObject
    {
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public Color Color { get; private set; }
    }
}