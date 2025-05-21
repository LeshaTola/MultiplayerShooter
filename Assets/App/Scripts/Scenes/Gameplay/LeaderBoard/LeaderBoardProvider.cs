using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.UserStats.Rank;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoardProvider 
    {
        private UserRankProvider _userRankProvider;

        public int Kills { get; private set; } = 0;
        public int Death { get; private set; } = 0;

        public int MyPlace
        {
            get
            {
                var table = GetTable();
                return table.FindIndex(x => x.Item2.Equals(PhotonNetwork.NickName)) + 1;
            }
        }

        public static LeaderBoardProvider Instance {get; private set;}

        public LeaderBoardProvider(UserRankProvider userRankProvider)
        {
            _userRankProvider = userRankProvider;
            Instance = this;
            UpdateTable();
        }

        public void AddKill()
        {
            Kills++;
            UpdateTable();
        }

        public void AddDeath()
        {
            Death++;
            UpdateTable();
        }

        public void Reset()
        {
            Kills = 0;
            Death = 0;
            UpdateTable();
        }

        public List<(int, string, int, int, int, bool)> GetTable()
        {
            List<(int, string, int, int, int, bool)> result = new();

            var sortedPlayers = PhotonNetwork.PlayerList
                .OrderByDescending(player =>
                {
                    if (player?.CustomProperties != null && player.CustomProperties.TryGetValue("Kills", out var killValue))
                    {
                        return killValue is int kills ? kills : 0;
                    }

                    return 0;
                })
                .ToList();

            foreach (var player in sortedPlayers)
            {
                var properties = player?.CustomProperties;
                string nickname = player?.NickName ?? "Unknown";
                bool isLocal = PhotonNetwork.LocalPlayer.Equals(player);

                int rank = (properties != null && properties.TryGetValue("Rank", out var rankVal) && rankVal is int r) ? r : 0;
                int kills = (properties != null && properties.TryGetValue("Kills", out var killsVal) && killsVal is int k) ? k : 0;
                int deaths = (properties != null && properties.TryGetValue("Death", out var deathsVal) && deathsVal is int d) ? d : 0;
                int ping = (properties != null && properties.TryGetValue("Ping", out var pingVal) && pingVal is int p) ? p : 0;

                result.Add((rank, nickname, kills, deaths, ping, isLocal));
            }

            return result;
        }

        private void UpdateTable()
        {
            var pingProp = new ExitGames.Client.Photon.Hashtable
            {
                ["Rank"] = _userRankProvider.CurrentRankId,
                ["Ping"] = PhotonNetwork.GetPing(),
                ["Kills"] = Kills,
                ["Death"] = Death
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(pingProp);
        }
    }
}