using System;
using App.Scripts.Features.Inventory;
using App.Scripts.Scenes.MainMenu.Features.UserStats;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats
{
    [Serializable]
    public class UserStatsData
    {
        public int Experience;
        public int CurrentRankId;

        public GameInventoryData GameInventory;
        public InventoryData Inventory;

        public int Coins;
        public int Tickets;
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public UserStatsData UserStatsData;
    }
}