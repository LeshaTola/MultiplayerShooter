using System;
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
using GameAnalyticsSDK;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market
{
    public class MarketService : ISavable, IInitializable, IUpdatable, ICleanupable
    {
        public event Action<float, float> OnTimerUpdated;
        public event Action<List<ShopItemData>, List<ShopItemData>> OnItemsUpdated;
        public event Action<int> OnCurrencyCountUpdated;

        private readonly GlobalInventory _globalInventory;
        private readonly RaritiesDatabase _raritiesDatabase;
        private readonly CostsDatabase _costsDatabase;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly IDataProvider<MarketSavesData> _dataProvider;

        private DateTime _lastUpdate;
        private int _currentCurrencyElementsCount;

        public float RemainingTime { get; private set; }
        public List<ShopItemData> CurrentWeapons { get; private set; } = new();
        public List<ShopItemData> CurrentSkins { get; private set; } = new();

        public int CurrentCurrencyElementsCount
        {
            get => _currentCurrencyElementsCount;
            set
            {
                _currentCurrencyElementsCount = value;
                OnCurrencyCountUpdated?.Invoke(_currentCurrencyElementsCount);
                SaveState();
            }
        }

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
                Weapons = CurrentWeapons.Select(x => x.Item.Id).ToList(),
                Skins = CurrentSkins.Select(x => x.Item.Id).ToList(),
                CurrentCurrencyElementsCount = CurrentCurrencyElementsCount,
            });
        }

        public void LoadState()
        {
            if (!_dataProvider.HasData())
            {
                _dataProvider.SaveData(new MarketSavesData
                {
                    LastUpdate = default,
                    Weapons = new List<string>(),
                    Skins = new List<string>(),
                    CurrentCurrencyElementsCount = 2
                });
            }

            var marketData = _dataProvider.GetData();
            _lastUpdate = marketData.LastUpdate == 0 ? DateTime.Now.AddHours(-2) : new DateTime(marketData.LastUpdate);

            CurrentWeapons.Clear();
            foreach (var itemId in marketData.Weapons)
            {
                var item = _userStatsProvider.InventoryProvider.GetConfigById(itemId);
                CurrentWeapons.Add(new ShopItemData
                {
                    Price = _costsDatabase.PriceByRarity[item.Rarity],
                    Item = item
                });
            }

            CurrentSkins.Clear();
            foreach (var itemId in marketData.Skins)
            {
                var item = _userStatsProvider.InventoryProvider.GetConfigById(itemId);
                CurrentSkins.Add(new ShopItemData
                {
                    Price = _costsDatabase.PriceByRarity[item.Rarity],
                    Item = item
                });
            }

            OnItemsUpdated?.Invoke(CurrentWeapons, CurrentSkins);

            CurrentCurrencyElementsCount = marketData.CurrentCurrencyElementsCount;
            OnCurrencyCountUpdated?.Invoke(CurrentCurrencyElementsCount);
            UpdateRemainingTime();
        }

        public void UpdateItems()
        {
            CurrentCurrencyElementsCount = 2;
            var inventory = _userStatsProvider.InventoryProvider.Inventory;

            var newWeapons = new List<ShopItemData>();
            var newSkins = new List<ShopItemData>();
            foreach (var rarity in _raritiesDatabase.Rarities.Values)
            {
                if (!_globalInventory.ItemsByRarity.TryGetValue(rarity.Name, out var items))
                {
                    continue;
                }

                var availableItems = items.Where(x => inventory.IsAvailable(x.Id) && !x.IsNotForSale).ToList();
                if (availableItems.Count <= 0)
                {
                    continue;
                }

                var skins = availableItems.Where(x => x is SkinConfig).ToList();
                if (skins.Count > 0)
                {
                    newSkins.Add(GetShopItemData(skins[Random.Range(0, skins.Count)]));
                }

                var weapons = availableItems.Where(x => x is WeaponConfig).ToList();
                if (weapons.Count > 0)
                {
                    newWeapons.Add(GetShopItemData(weapons[Random.Range(0, weapons.Count)]));
                }
            }

            UpdateItemsInternal(newWeapons, newSkins);
        }

        public ShopItemData GetShopItemDataByMarket(ItemConfig item)
        {
            var shopItemData = GetShopItemData(item);
            var isInMarket = IsInMarket(item);
            if (!isInMarket)
            {
                shopItemData.Price *= 2;
            }
            
            return shopItemData;
        }

        public bool TryBuyItem(ItemConfig item)
        {
            var shopItemData = GetShopItemData(item);
            return TryBuyItem(shopItemData);
        }

        public bool TryBuyItem(ShopItemData shopItemData)
        {
            if (!_userStatsProvider.CoinsProvider.IsEnough(shopItemData.Price))
            {
                return false;
            }

            ChangeMoney(shopItemData);
            AddItem(shopItemData);
            UpdateInventory();
            GameAnalytics.NewDesignEvent($"shop:{shopItemData.Item.Id.ToLower()}", 1);
            return true;
        }

        private bool IsInMarket(ItemConfig item)
        {
            var isInMarket = CurrentWeapons.FirstOrDefault(x => x.Item.Id.Equals(item.Id)) != null
                             || CurrentSkins.FirstOrDefault(x => x.Item.Id.Equals(item.Id)) != null;
            return isInMarket;
        }

        private ShopItemData GetShopItemData(ItemConfig item)
        {
            return new ShopItemData
            {
                Price = _costsDatabase.PriceByRarity[item.Rarity],
                Item = item
            };
        }

        private void ChangeMoney(ShopItemData shopItemData)
        {
            _userStatsProvider.CoinsProvider.ChangeCoins(-shopItemData.Price);
        }

        private void AddItem(ShopItemData shopItemData)
        {
            switch (shopItemData.Item)
            {
                case WeaponConfig weaponConfig:
                    _userStatsProvider.InventoryProvider.Inventory.Weapons.Add(weaponConfig.Id);
                    break;
                case SkinConfig skinConfig:
                    _userStatsProvider.InventoryProvider.Inventory.Skins.Add(skinConfig.Id);
                    break;
            }
        }

        private void UpdateInventory()
        {
            _userStatsProvider.InventoryProvider.Inventory.InvokeInventoryUpdate();
            _userStatsProvider.SaveState();
        }

        private void UpdateItemsInternal(List<ShopItemData> newItems, List<ShopItemData> newSkins)
        {
            CurrentWeapons = newItems;
            CurrentSkins = newSkins;
            OnItemsUpdated?.Invoke(CurrentWeapons, CurrentSkins);
            SaveState();
        }

        private void UpdateRemainingTime()
        {
            DateTime now = DateTime.Now;

            int periodStartHour = (now.Hour / 2) * 2;
            DateTime currentPeriodStart = new DateTime(now.Year, now.Month, now.Day, periodStartHour, 0, 0);
            DateTime currentPeriodEnd = currentPeriodStart.AddHours(2);

            int lastPeriodStartHour = (_lastUpdate.Hour / 2) * 2;
            DateTime lastPeriodStart = new DateTime(_lastUpdate.Year, _lastUpdate.Month, _lastUpdate.Day,
                lastPeriodStartHour, 0, 0);

            if (_lastUpdate.Date != now.Date || lastPeriodStart != currentPeriodStart)
            {
                RemainingTime = 0;
                _lastUpdate = now;
                return;
            }

            TimeSpan timeLeft = currentPeriodEnd - now;
            RemainingTime = (float) timeLeft.TotalSeconds;
            _lastUpdate = now;
        }


        private void ValidateCurrentItems()
        {
            var inventory = _userStatsProvider.InventoryProvider.Inventory;
            var availableWeapons = GetAvailableWeapons(inventory);
            var availableSkins = GetAvailableSkins(inventory);
            UpdateItemsInternal(availableWeapons, availableSkins);
        }

        private List<ShopItemData> GetAvailableSkins(InventoryData inventory)
        {
            var availableItems = CurrentSkins
                .Where(item =>
                    inventory.IsAvailable(item.Item.Id) && !item.Item.IsNotForSale && item.Item is SkinConfig).ToList();
            return availableItems;
        }

        private List<ShopItemData> GetAvailableWeapons(InventoryData inventory)
        {
            var availableItems = CurrentWeapons
                .Where(item =>
                    inventory.IsAvailable(item.Item.Id) && !item.Item.IsNotForSale && item.Item is WeaponConfig)
                .ToList();
            return availableItems;
        }
    }
}