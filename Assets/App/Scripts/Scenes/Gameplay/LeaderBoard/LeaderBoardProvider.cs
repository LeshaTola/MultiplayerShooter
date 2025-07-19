using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.UserStats.Rank;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoardProvider
    {
        private readonly UserRankProvider _userRankProvider;
        private readonly InventoryProvider _inventoryProvider;

        public int Kills { get; private set; } = 0;
        public int Death { get; private set; } = 0;

        public int MyPlace
        {
            get
            {
                var table = GetTable();
                return table.FindIndex(x => x.NickName.Equals(PhotonNetwork.NickName)) + 1;
            }
        }

        public static LeaderBoardProvider Instance { get; private set; }

        public LeaderBoardProvider(UserRankProvider userRankProvider, InventoryProvider inventoryProvider)
        {
            _userRankProvider = userRankProvider;
            _inventoryProvider = inventoryProvider;
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

        public List<LeaderBoardData> GetTable()
        {
            return PhotonNetwork.PlayerList
                .OrderByDescending(player =>
                {
                    if (player.CustomProperties != null &&
                        player.CustomProperties.TryGetValue("Kills", out var killsValue) &&
                        killsValue is int kills)
                    {
                        return kills;
                    }

                    return 0;
                })
                .Select(player =>
                {
                    var properties = player.CustomProperties;

                    T GetValue<T>(string key, T defaultValue = default)
                    {
                        return properties != null &&
                               properties.TryGetValue(key, out var value) &&
                               value is T typedValue
                            ? typedValue
                            : defaultValue;
                    }

                    return new LeaderBoardData
                    {
                        NickName = player.NickName ?? "Unknown",
                        RankId = GetValue("Rank", 0),
                        Kills = GetValue("Kills", 0),
                        Death = GetValue("Death", 0),
                        Ping = GetValue("Ping", 0),
                        SkinId = GetValue("Skin", string.Empty),
                        IsMobile = GetValue("IsMobile", false),
                        IsMine = PhotonNetwork.LocalPlayer.Equals(player)
                    };
                })
                .ToList();
        }

        public void UpdateTable()
        {
            var playerProperty = new ExitGames.Client.Photon.Hashtable
            {
                ["IsMobile"] = _userRankProvider.CurrentRankId,
                ["Skin"] = _inventoryProvider.GameInventory.Skin,
                ["Rank"] = _userRankProvider.CurrentRankId,
                ["Ping"] = PhotonNetwork.GetPing(),
                ["Kills"] = Kills,
                ["Death"] = Death
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperty);
        }
    }

    public class LeaderBoardData
    {
        public string SkinId;
        public int RankId;
        public bool IsMobile;
        public string NickName;
        public int Kills;
        public int Death;
        public int Ping;
        public bool IsMine;

        public Sprite SkinSprite;
        public Color SkinColor = Color.white;
        public Sprite RankSprite;
    }
}