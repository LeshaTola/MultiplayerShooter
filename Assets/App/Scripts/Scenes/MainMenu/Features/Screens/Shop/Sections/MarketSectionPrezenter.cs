using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections
{
    public class MarketSectionPrezenter
    {
        private readonly MarketSectionView _view;
        private readonly MarketService _marketService;
        private readonly ILocalizationSystem _localizationSystem;

        public MarketSectionPrezenter(MarketSectionView view,
            MarketService marketService,
            ILocalizationSystem localizationSystem)
        {
            _view = view;
            _marketService = marketService;
            _localizationSystem = localizationSystem;
        }

        public void Initialzie()
        {
            _view.Initialzie(_localizationSystem);

            _marketService.OnItemsUpdated += _view.UpdateSections;
            _marketService.OnTimerUpdated += _view.UpdateTimer;
            _view.OnItemClicked += OnItemClicked;
            _view.OnUpdateButtonClicked += OnUpdateButtonClicked;
        }

        public void Cleanup()
        {
            _view.Cleanup();

            _marketService.OnItemsUpdated -= _view.UpdateSections;
            _marketService.OnTimerUpdated -= _view.UpdateTimer;
            _view.OnItemClicked -= OnItemClicked;
            _view.OnUpdateButtonClicked += OnUpdateButtonClicked;
        }

        private void OnUpdateButtonClicked()
        {
            _marketService.UpdateItems();
        }

        private void OnItemClicked(int id)
        {
        }
    }

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
            // LoadState();
        }

        public void Cleanup()
        {
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
            _dataProvider.SaveData(new()
            {
                LastUpdate = _lastUpdate,
                CurrentItems = CurrentItems.Select(x=>x.Item.Id).ToList(),
            });
        }

        public void LoadState()
        {
            if (!_dataProvider.HasData())
            {
                _dataProvider.SaveData(new()
                {
                    LastUpdate = default,
                    CurrentItems = new List<string>(),
                });
            }

            var marketData = _dataProvider.GetData();
            _lastUpdate = marketData.LastUpdate;
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

            CurrentItems.Clear();
            foreach (var rarity in _raritiesDatabase.Rarities.Values)
            {
                if (!_globalInventory.ItemsByRarity.TryGetValue(rarity.Name, out var items))
                {
                    continue;
                }

                var availableItems = items
                    .Where(item => !inventory.Skins.Contains(item.Id) &&
                                   !inventory.Weapons.Contains(item.Id) &&
                                   !inventory.Equipment.Contains(item.Id)).ToList();
                if (availableItems.Count <= 0)
                {
                    continue;
                }

                CurrentItems.Add(new ShopItemData
                {
                    Price = _costsDatabase.PriceByRarity[rarity.Name],
                    Item = availableItems[Random.Range(0, availableItems.Count)]
                });
                
                OnItemsUpdated?.Invoke(CurrentItems);
                SaveState();
            }
        }

        private void UpdateRemainingTime()
        {
            if (DateTime.Now - _lastUpdate > TimeSpan.FromSeconds(7200))
            {
                RemainingTime = 0;
                _lastUpdate = DateTime.Now;
                return;
            }
            
            DateTime now = DateTime.Now;
            int periodStart = (now.Hour / 2) * 2;
            DateTime periodStartTime = new DateTime(now.Year, now.Month, now.Day, periodStart, 0, 0);
            DateTime periodEndTime = periodStartTime.AddHours(2);
            TimeSpan timeLeft = periodEndTime - now;
            var newRemainingTime = (float) timeLeft.TotalSeconds;
            RemainingTime = newRemainingTime;
            
            _lastUpdate = periodStartTime;
        }
    }

    [Serializable]
    public class MarketSavesData
    {
        public List<string> CurrentItems;
        public DateTime LastUpdate;
    }
}