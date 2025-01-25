using App.Scripts.Features.Inventory.Configs;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards.Configs
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/Inventory/Reward")]
    public class RewardConfig : ScriptableObject
    {
        [field: SerializeField] public ItemConfig Reward { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
    }
}