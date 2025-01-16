using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.PlayerStats;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoardProvider 
    {
        private UserStatsProvider _userStatsProvider;
        
        private int _kills = 0;
        private int _death = 0;

        public static LeaderBoardProvider Instance {get; private set;}
        
        public LeaderBoardProvider(UserStatsProvider userStatsProvider)
        {
            _userStatsProvider = userStatsProvider;
            Instance = this;
            UpdateTable();
        }

        public void AddKill()
        {
            _kills++;
            UpdateTable();
        }

        public void AddDeath()
        {
            _death++;
            UpdateTable();
        }

        public List<(int, string, int, int, int, bool)> GetTable()
        {
            List<(int, string, int, int, int, bool)> result = new();

            var sortedPlayers = PhotonNetwork.PlayerList
                .OrderByDescending(player =>
                {
                    if (player.CustomProperties.TryGetValue("Kills", out var killValue))
                    {
                        return (int) killValue;
                    }

                    return 0;
                }).ToList();

            foreach (var player in sortedPlayers)
            {
                result.Add(new ValueTuple<int, string, int, int, int, bool>((int) player.CustomProperties["Rank"],
                    player.NickName,
                    (int) player.CustomProperties["Kills"],
                    (int) player.CustomProperties["Death"],
                    (int) player.CustomProperties["Ping"],
                    PhotonNetwork.LocalPlayer.Equals(player)));
            }

            return result;
        }

        private void UpdateTable()
        {
            var pingProp = new ExitGames.Client.Photon.Hashtable
            {
                ["Rank"] = _userStatsProvider.CurrentRankId,
                ["Ping"] = PhotonNetwork.GetPing(),
                ["Kills"] = _kills,
                ["Death"] = _death
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(pingProp);
        }
    }
}