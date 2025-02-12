using System.Collections.Generic;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.UserStats.Rank.Configs
{
    [CreateAssetMenu(fileName = "RanksDatabase", menuName = "Databases/Ranks")]
    public class RanksDatabase:ScriptableObject
    {
        [field: SerializeField] public List<RankData> Ranks { get; private set; }

        [field:Space]
        [field: SerializeField] public float StartExpValue { get; private set; } = 100;
        [field: SerializeField] public float ExpMultiplier { get; private set; } = 2;
        
        [Button]
        public void SetExp()
        {
            int i = 1;
            foreach (var rankData in Ranks)
            {
                rankData.ExpForRank = (int) (StartExpValue * Mathf.Pow(i,ExpMultiplier));
                i++;
            }
        }
    }
}