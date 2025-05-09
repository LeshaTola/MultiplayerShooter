﻿using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Screen
{
    public class InventoryScreenPresenter : GameScreenPresenter , IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private readonly InventoryScreeen _inventoryScreen;
        private readonly TabSwitcher _tabSwitcher;
        private readonly GameInventoryViewPresenter _gameInventoryViewPresenter;
        private readonly List<InventoryTabPresenter> _inventoryTabPresenters;
        private readonly UserStatsView _userStatsView;
        private readonly MarketViewPresenter _marketViewPresenter;
        private readonly ISoundProvider _soundProvider;
        private readonly SelectionProvider _selectionProvider;

        public InventoryScreenPresenter(InventoryScreeen inventoryScreen,
            TabSwitcher tabSwitcher,
            GameInventoryViewPresenter gameInventoryViewPresenter,
            List<InventoryTabPresenter> inventoryTabPresenters,
            UserStatsView userStatsView,
            MarketViewPresenter marketViewPresenter, 
            ISoundProvider soundProvider, 
            SelectionProvider selectionProvider)
        {
            _inventoryScreen = inventoryScreen;
            _tabSwitcher = tabSwitcher;
            _gameInventoryViewPresenter = gameInventoryViewPresenter;
            _inventoryTabPresenters = inventoryTabPresenters;
            _userStatsView = userStatsView;
            _marketViewPresenter = marketViewPresenter;
            _soundProvider = soundProvider;
            _selectionProvider = selectionProvider;
        }

        public override void Initialize()
        {
            _tabSwitcher.Initialize();
            _tabSwitcher.OnTabSwitched += OnTabSwitched;
            _selectionProvider.OnSkinSelected += OnSkinSelected;
            _selectionProvider.OnWeaponSelected += OnSkinSelected;
            
            foreach (var inventoryTabPresenter in _inventoryTabPresenters)
            {
                inventoryTabPresenter.Initialize();
            }
            _gameInventoryViewPresenter.Initialize();
            _inventoryScreen.Initialize();
            
            _marketViewPresenter.Initialize();
        }

        public override void Cleanup()
        {
            foreach (var inventoryTabPresenter in _inventoryTabPresenters)
            {
                inventoryTabPresenter.Cleanup();
            }
            
            _tabSwitcher.Cleanup();
            _tabSwitcher.OnTabSwitched -= OnTabSwitched;
            _selectionProvider.OnSkinSelected -= OnSkinSelected;
            _selectionProvider.OnWeaponSelected -= OnSkinSelected;
            
            _gameInventoryViewPresenter.Cleanup();
            _inventoryScreen.Cleanup();
            _marketViewPresenter.Cleanup();
        }

        public override async UniTask Show()
        {
            _tabSwitcher.Show();
            var tasks = new List<UniTask>
            {
                _userStatsView.Show(),
                _inventoryScreen.Show()
            };
            await UniTask.WhenAll(tasks);
        }

        public override async UniTask Hide()
        {
            var tasks = new List<UniTask>
            {
                _userStatsView.Hide(),
                _inventoryScreen.Hide()
            };
            await UniTask.WhenAll(tasks);
            _tabSwitcher.Hide();
        }

        private void OnTabSwitched()
        {
            _soundProvider.PlaySound(_inventoryScreen.ToggleSond);
        }

        private void OnSkinSelected(string obj)
        {
            _soundProvider.PlaySound(_inventoryScreen.SelectWeaponSound);
        }
    }
}