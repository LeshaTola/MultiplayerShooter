﻿using System;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using App.Scripts.Features.UserStats.Rank.Configs;
using App.Scripts.Scenes.MainMenu.Features.UserStats;

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
            if (experience < 0)
            {
                return 0;
            }

            int levelUps = 0;
            experience += Experience;

            while (experience >= CurrentRank.ExpForRank)
            {
                experience -= CurrentRank.ExpForRank;
                CurrentRankId++;
                levelUps++;
            }

            Experience = experience;
            OnExperienceChanded?.Invoke();
            return levelUps;
        }

        public void SetState(UserStatsData userStats)
        {
            Experience = userStats.Experience;
            CurrentRankId = userStats.CurrentRankId;
            OnExperienceChanded?.Invoke();
        }
    }
}