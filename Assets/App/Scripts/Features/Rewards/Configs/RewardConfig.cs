using App.Scripts.Features.Inventory.Configs;
using UnityEngine;

namespace App.Scripts.Features.Rewards.Configs
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/Rewards/Reward")]
    public class RewardConfig : ScriptableObject
    {
        [field: SerializeField] public ItemConfig Reward { get; private set; }
        [field: SerializeField] public int Count { get; set; }
    }
}