using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections
{
    public class MarketSectionPrezenter
    {
        private readonly MarketSectionView _view;
        private readonly GlobalInventory _globalInventory;
        private readonly RaritiesDatabase _raritiesDatabase;
        private readonly CostsDatabase _costsDatabase;
        private readonly UserStatsProvider _userStatsProvider;
        
        private readonly List<ShopItemData> _itemsList = new();

        public MarketSectionPrezenter(MarketSectionView view, 
            GlobalInventory globalInventory,
            RaritiesDatabase raritiesDatabase,
            CostsDatabase costsDatabase, 
            UserStatsProvider userStatsProvider)
        {
            _view = view;
            _globalInventory = globalInventory;
            _raritiesDatabase = raritiesDatabase;
            _costsDatabase = costsDatabase;
            _userStatsProvider = userStatsProvider;
        }
        
        public void Initialzie()
        {
            _view.Initialzie();
            _view.OnItemClicked += OnItemClicked;
            _view.OnUpdateButtonClicked += OnUpdateButtonClicked;
        }

        public void Cleanup()
        {
            _view.Cleanup();
            _view.OnItemClicked -= OnItemClicked;
            _view.OnUpdateButtonClicked += OnUpdateButtonClicked;
        }

        public void UpdateSections()
        {
            var inventory = _userStatsProvider.InventoryProvider.Inventory;

            _itemsList.Clear();
            foreach (var rarity in _raritiesDatabase.Rarities.Values)
            {
                if (!_globalInventory.ItemsByRarity.ContainsKey(rarity.Name))
                {
                    continue;
                }
                
                var availableItems = _globalInventory.ItemsByRarity[rarity.Name]
                    .Where(item => !inventory.Skins.Contains(item.Id) &&
                                   !inventory.Weapons.Contains(item.Id) &&
                                   !inventory.Equipment.Contains(item.Id)).ToList();
                if (availableItems.Count <= 0)
                {
                    Debug.Log("Skip");
                    continue;
                }
                
                Debug.Log(rarity.Name);
                _itemsList.Add(new ShopItemData
                {
                    Price = _costsDatabase.PriceByRarity[rarity.Name],
                    Item = availableItems[Random.Range(0, availableItems.Count)]
                });
            }
            
            _view.UpdateSections(_itemsList);
        }
        
        private void OnUpdateButtonClicked()
        {
            UpdateSections();
        }
        
        private void OnItemClicked(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}