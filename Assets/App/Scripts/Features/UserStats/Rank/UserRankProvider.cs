using System;
using App.Scripts.Features.PlayerStats.Rank.Configs;

namespace App.Scripts.Features.PlayerStats
{
    public class UserRankProvider
    {
        public event Action OnExperienceChanded;
        
        public RanksDatabase RanksDatabase { get; }
        public int Experience { get; private set; }
        public int CurrentRankId { get; private set; }
        public RankData CurrentRank =>RanksDatabase.Ranks[CurrentRankId];
        
        public UserRankProvider(RanksDatabase ranksDatabase)
        {
            RanksDatabase = ranksDatabase;
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
            CurrentRankId += levelUps;
            
            OnExperienceChanded?.Invoke();
            return levelUps;
        }
    }
}