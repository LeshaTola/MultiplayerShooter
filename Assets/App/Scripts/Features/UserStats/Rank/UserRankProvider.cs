using System;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using App.Scripts.Features.UserStats.Rank.Configs;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using YG;

namespace App.Scripts.Features.UserStats.Rank
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
            if (experience < 0)
            {
                return 0;
            }

            var levelUps = AddExpInternal(experience);

            OnExperienceChanded?.Invoke();
            UpdateLeaderBoard();
            
            return levelUps;
        }

        private int AddExpInternal(int experience)
        {
            var levelUps = 0;

            experience += Experience;
            while (experience >= CurrentRank.ExpForRank)
            {
                experience -= CurrentRank.ExpForRank;
                CurrentRankId++;
                levelUps++;
            }
            Experience = experience;
            return levelUps;
        }

        public void SetState(UserStatsData userStats)
        {
            Experience = userStats.Experience;
            CurrentRankId = userStats.CurrentRankId;
            
            OnExperienceChanded?.Invoke();
            UpdateLeaderBoard();
        }

        private void UpdateLeaderBoard()
        {
            YG2.SetLeaderboard("ranks",CurrentRankId + 1);
        }
    }
}