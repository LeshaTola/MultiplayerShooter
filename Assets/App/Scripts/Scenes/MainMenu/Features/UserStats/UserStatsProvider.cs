using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Data;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Modules.Saves;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats
{
    public class UserStatsProvider : ISavable
    {
        private readonly InventoryConfig _inventoryConfig;
        private readonly IDataProvider<UserStatsData> _dataProvider;

        public UserRankProvider RankProvider { get; }
        public InventoryProvider InventoryProvider { get; }
        public CoinsProvider CoinsProvider { get; }
        public TicketsProvider TicketsProvider { get; }

        public UserStatsProvider(UserRankProvider rankProvider,
            InventoryProvider inventoryProvider,
            CoinsProvider coinsProvider,
            TicketsProvider ticketsProvider,
            IDataProvider<UserStatsData> dataProvider,
            InventoryConfig inventoryConfig)
        {
            _dataProvider = dataProvider;
            _inventoryConfig = inventoryConfig;
            RankProvider = rankProvider;
            InventoryProvider = inventoryProvider;
            CoinsProvider = coinsProvider;
            TicketsProvider = ticketsProvider;
        }

        public void SaveState()
        {
            _dataProvider.SaveData(GetState());
        }

        public void LoadState()
        {
            if (!_dataProvider.HasData())
            {
                _dataProvider.SaveData(new UserStatsData()
                {
                    CurrentRankId = 0,
                    Experience = 0,
                    Coins = 0,
                    Tickets = 0,

                    GameInventory = new GameInventoryData()
                    {
                        Skin = _inventoryConfig.Skin?.Id ?? "",
                        Weapons = _inventoryConfig.Weapons.Select(x=>x?.Id ?? "").ToList(),
                        Equipment = _inventoryConfig.Equipment.Select(x=>x?.Id ?? "").ToList()
                    },

                    Inventory = new ()
                    {
                        Skins = _inventoryConfig.PlayerInventory.SkinConfigs.Select(x=>x?.Id ?? "").ToList(),
                        Weapons = _inventoryConfig.PlayerInventory.Weapons.Select(x=>x?.Id ?? "").ToList(),
                        Equipment = _inventoryConfig.PlayerInventory.Equipment.Select(x=>x?.Id ?? "").ToList()
                    }
                });
            }

            SetState(_dataProvider.GetData());
        }

        private UserStatsData GetState()
        {
            return new UserStatsData
            {
                Experience = RankProvider.Experience,
                CurrentRankId = RankProvider.CurrentRankId,
                Coins = CoinsProvider.Coins,
                Tickets = TicketsProvider.Tickets,

                GameInventory = InventoryProvider.GameInventory,
                Inventory = InventoryProvider.Inventory.ToSavesData(),
            };
        }

        private void SetState(UserStatsData data)
        {
            RankProvider.SetState(data);
            CoinsProvider.ChangeCoins(data.Coins);
            TicketsProvider.ChangeTickets(data.Tickets);

            InventoryProvider.SetState(data);
        }
    }
}