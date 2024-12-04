using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.UI.LeaderBoard
{
    public class LeaderBoardProvider : MonoBehaviour
    {
        private int _kills = 0;
        private int _death = 0;

        public static LeaderBoardProvider Instance { get; private set; }

        private void Start()
        {
            UpdateTable();
            Instance = this;
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

        public List<(string, int, int, int, bool)> GetTable()
        {
            List<(string, int, int, int, bool)> result = new();

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
                result.Add(new ValueTuple<string, int, int, int, bool>(player.NickName,
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
                ["Ping"] = PhotonNetwork.GetPing(),
                ["Kills"] = _kills,
                ["Death"] = _death
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(pingProp);
        }
    }
}