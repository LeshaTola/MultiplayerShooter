﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Inventory.Data;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market
{
    public class MarketService : ISavable, IInitializable, IUpdatable, ICleanupable
    {
        public event Action<float, float> OnTimerUpdated;
        public event Action<List<ShopItemData>> OnItemsUpdated;

        private readonly GlobalInventory _globalInventory;
        private readonly RaritiesDatabase _raritiesDatabase;
        private readonly CostsDatabase _costsDatabase;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly IDataProvider<MarketSavesData> _dataProvider;

        private DateTime _lastUpdate;

        public float RemainingTime { get; private set; }
        public List<ShopItemData> CurrentItems { get; private set; } = new();

        public MarketService(
            GlobalInventory globalInventory,
            RaritiesDatabase raritiesDatabase,
            CostsDatabase costsDatabase,
            UserStatsProvider userStatsProvider,
            IDataProvider<MarketSavesData> dataProvider)
        {
            _globalInventory = globalInventory;
            _raritiesDatabase = raritiesDatabase;
            _costsDatabase = costsDatabase;
            _userStatsProvider = userStatsProvider;
            _dataProvider = dataProvider;
        }

        public void Initialize()
        {
            _userStatsProvider.InventoryProvider.Inventory.OnInventoryUpdated += ValidateCurrentItems;
        }

        public void Cleanup()
        {
            _userStatsProvider.InventoryProvider.Inventory.OnInventoryUpdated -= ValidateCurrentItems;
            SaveState();
        }

        public void Update()
        {
            RemainingTime -= Time.deltaTime;
            if (RemainingTime <= 0)
            {
                UpdateItems();
                UpdateRemainingTime();
            }

            OnTimerUpdated?.Invoke(RemainingTime, 7200);
        }

        public void SaveState()
        {
            _dataProvider.SaveData(new MarketSavesData
            {
                LastUpdate = _lastUpdate.Ticks,
                CurrentItems = CurrentItems.Select(x => x.Item.Id).ToList(),
            });
        }

        public void LoadState()
        {
            if (!_dataProvider.HasData())
            {
                _dataProvider.SaveData(new MarketSavesData
                {
                    LastUpdate = default,
                    CurrentItems = new List<string>(),
                });
            }

            var marketData = _dataProvider.GetData();
            if (marketData.LastUpdate == 0)
            {
                _lastUpdate = DateTime.Now.AddHours(-2);
            }
            else
            {
                _lastUpdate = new DateTime(marketData.LastUpdate);
            }
            
            CurrentItems.Clear();
            foreach (var itemId in marketData.CurrentItems)
            {
                var item = _userStatsProvider.InventoryProvider.GetConfigById(itemId);
                CurrentItems.Add(new ShopItemData
                {
                    Price = _costsDatabase.PriceByRarity[item.Rarity],
                    Item = item
                });
            }

            OnItemsUpdated?.Invoke(CurrentItems);
            UpdateRemainingTime();
        }

        public void UpdateItems()
        {
            var inventory = _userStatsProvider.InventoryProvider.Inventory;

            var newItems = new List<ShopItemData>();
            foreach (var rarity in _raritiesDatabase.Rarities.Values)
            {
                if (!_globalInventory.ItemsByRarity.TryGetValue(rarity.Name, out var items))
                {
                    continue;
                }

                var availableItems = items.Where(x=>inventory.IsAvailable(x.Id) && !x.IsNotForSale).ToList();
                if (availableItems.Count <= 0)
                {
                    continue;
                }

                newItems.Add(new ShopItemData
                {
                    Price = _costsDatabase.PriceByRarity[rarity.Name],
                    Item = availableItems[Random.Range(0, availableItems.Count)]
                });
            }

            UpdateItemsInternal(newItems);
        }

        private void UpdateItemsInternal(List<ShopItemData> newItems)
        {
            CurrentItems = newItems;
            OnItemsUpdated?.Invoke(CurrentItems);
            SaveState();
        }

        private void UpdateRemainingTime()
        {
            DateTime now = DateTime.Now;

            int periodStartHour = (now.Hour / 2) * 2;
            DateTime currentPeriodStart = new DateTime(now.Year, now.Month, now.Day, periodStartHour, 0, 0);
            DateTime currentPeriodEnd = currentPeriodStart.AddHours(2);

            int lastPeriodStartHour = (_lastUpdate.Hour / 2) * 2;
            DateTime lastPeriodStart = new DateTime(_lastUpdate.Year, _lastUpdate.Month, _lastUpdate.Day, lastPeriodStartHour, 0, 0);

            if (_lastUpdate.Date != now.Date || lastPeriodStart != currentPeriodStart)
            {
                RemainingTime = 0;
                _lastUpdate = now;
                return;
            }

            TimeSpan timeLeft = currentPeriodEnd - now;
            RemainingTime = (float)timeLeft.TotalSeconds;
            _lastUpdate = now;
        }


        private void ValidateCurrentItems()
        {
            var inventory = _userStatsProvider.InventoryProvider.Inventory;
            var availableItems = GetAvailableItems(inventory);
            UpdateItemsInternal(availableItems);
        }

        private List<ShopItemData> GetAvailableItems(InventoryData inventory)
        {
            var availableItems = CurrentItems
                .Where(item => inventory.IsAvailable(item.Item.Id) && !item.Item.IsNotForSale).ToList();
            return availableItems;
        }
    }
}