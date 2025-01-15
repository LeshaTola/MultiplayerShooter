using System;
using App.Scripts.Features.PlayerStats.Rank.Configs;

namespace App.Scripts.Features.PlayerStats
{
    public class UserStatsProvider
    {
        public event Action OnExperienceChanded;
        
        private readonly RanksDatabase _ranksDatabase;

        public int Experience { get; private set; }
        public int CurrentRankId { get; private set; }
        public RankData CurrentRank =>_ranksDatabase.Ranks[CurrentRankId];

        public int ExperienceToAdd { get; set; }

        public UserStatsProvider(RanksDatabase ranksDatabase)
        {
            _ranksDatabase = ranksDatabase;
        }

        public int AddExperience()
        {
            int levelUps = 0;
            if (ExperienceToAdd < 0)
            {
                return 0;
            }

            while (ExperienceToAdd > CurrentRank.ExpForRank)
            {
                levelUps++;
                ExperienceToAdd -= CurrentRank.ExpForRank;
            }
            Experience += ExperienceToAdd;
            
            OnExperienceChanded?.Invoke();
            return levelUps;
        }
    }
}