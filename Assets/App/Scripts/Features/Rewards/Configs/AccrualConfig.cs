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
        [field: SerializeField, Min(0)] public float GrowExpTime { get; private set; } = 120;
        [field: SerializeField, Min(0)] public float GrowExpValue { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float ExpPerKill { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float CoinsPerKill { get; private set; } = 1;

        
        [field: Header("Match End")]
        [field: SerializeField, Min(0)] public float MinMatchTime { get; private set; } = 120;
        
        [field:Space]
        [field: SerializeField, Min(0)] public float MaxExpPercent { get; private set; }
        
        
        [field: Header("Other")]
        [field: SerializeField] public RewardConfig CoinReward { get; private set; }
    }
}