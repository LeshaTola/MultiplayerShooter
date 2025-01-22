using System;
using App.Scripts.Features.PlayerStats.Rank.Configs;

namespace App.Scripts.Features.PlayerStats
{
    public class UserRankProvider
    {
        public event Action OnExperienceChanded;
        
        private readonly RanksDatabase _ranksDatabase;

        public int Experience { get; private set; }
        public int CurrentRankId { get; private set; }
        public RankData CurrentRank =>_ranksDatabase.Ranks[CurrentRankId];
        
        public UserRankProvider(RanksDatabase ranksDatabase)
        {
            _ranksDatabase = ranksDatabase;
        }
        
        public int AddExperience(int experience)
        {
            int levelUps = 0;
            if (experience < 0)
            {
                return 0;
            }

            while (experience > CurrentRank.ExpForRank)
            {
                levelUps++;
                experience -= CurrentRank.ExpForRank;
            }
            Experience += experience;
            
            OnExperienceChanded?.Invoke();
            return levelUps;
        }
    }
}