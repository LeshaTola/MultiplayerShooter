using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.PlayerStats.Rank.Configs
{
    [CreateAssetMenu(fileName = "RanksDatabase", menuName = "Databases/Ranks")]
    public class RanksDatabase:ScriptableObject
    {
        [field: SerializeField] public List<RankData> Ranks { get; set; }

        [Button]
        public void SetExp()
        {
            int i = 1;
            foreach (var rankData in Ranks)
            {
                rankData.ExpForRank = Random.Range(10*i*i, 20*i*i);
                i++;
            }
        }
    }
}