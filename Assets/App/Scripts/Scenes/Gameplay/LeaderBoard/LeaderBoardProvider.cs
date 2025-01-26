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
                result.Add(new ValueTuple<int, string, int, int, int, bool>(
                    (int) player.CustomProperties["Rank"],
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
                ["Rank"] = _userRankProvider.CurrentRankId,
                ["Ping"] = PhotonNetwork.GetPing(),
                ["Kills"] = Kills,
                ["Death"] = Death
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(pingProp);
        }
    }
}