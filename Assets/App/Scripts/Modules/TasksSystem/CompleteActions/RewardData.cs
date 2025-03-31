using System;
using UnityEngine;

namespace App.Scripts.Modules.TasksSystem.CompleteActions
{
    [Serializable]
    public class RewardData
    {
        public Sprite Sprite;
        public string Text;
        public string ValueText;
        public Color Color = Color.white;
    }
}