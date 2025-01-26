using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Features.Rewards.Configs
{
    [CreateAssetMenu(fileName = "AccrualConfig", menuName = "Configs/Rewards/Accrual")]
    public class AccrualConfig : ScriptableObject
    {
        [field: Header("Kills")]
        [field: SerializeField] public List<float> ExpPerKill { get; private set; }
        
        [field: SerializeField] public List<float> CoinsPerKill { get; private set; }

        [field: Header("Match End")]
        [field: SerializeField] public float MinMatchTime { get; private set; } = 120;
        
        [field:Space]
        [field: SerializeField] public List<float> ExpPerPlace { get; private set; }
        
        [field: SerializeField] public List<float> CoinsPerPlace { get; private set; }
        
        [field: Header("Other")]
        [field: SerializeField] public RewardConfig RewardConfig { get; private set; }
    }
}