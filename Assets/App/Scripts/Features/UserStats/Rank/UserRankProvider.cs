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
        public RankData NextRank => RanksDatabase.Ranks[CurrentRankId + 1];
        
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
                experience -= CurrentRank.ExpForRank;
                CurrentRankId ++;
                levelUps++; 
            }
            Experience += experience;
            
            OnExperienceChanded?.Invoke();
            return levelUps;
        }
    }
}