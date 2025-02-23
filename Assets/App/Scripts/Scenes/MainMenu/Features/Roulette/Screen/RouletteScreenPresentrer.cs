using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Screen
{
    public class RouletteScreenPresentrer : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly RouletteConfig _rouletteConfig;
        private readonly RouletteScreen _rouletteScreen;
        private readonly Roulette _roulette;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly RewardService _rewardService;
        private readonly InfoPopupRouter _infoPopupRouter;

        private Dictionary<SectorConfig, List<RewardConfig>> _winItems;
        
        public RouletteScreenPresentrer(RouletteConfig rouletteConfig,
            RouletteScreen rouletteScreen,
            Roulette roulette,
            UserStatsProvider userStatsProvider,
            RewardService rewardService,
            InfoPopupRouter infoPopupRouter)
        {
            _rouletteConfig = rouletteConfig;
            _rouletteScreen = rouletteScreen;
            _roulette = roulette;
            _userStatsProvider = userStatsProvider;
            _rewardService = rewardService;
            _infoPopupRouter = infoPopupRouter;
        }

        public override void Initialize()
        {
            _rouletteScreen.SpinButtonPressed += OnSpinButtonPressed;
            _userStatsProvider.TicketsProvider.OnTicketsChanged += _rouletteScreen.SetupTicketsCount;
            _rouletteScreen.SetupSectors(_rouletteConfig);
            
            UpdateWinItems();
            _rouletteScreen.Initialize();
            _roulette.GenerateRoulette();
        }

        public override void Cleanup()
        {
            _rouletteScreen.SpinButtonPressed -= OnSpinButtonPressed;
            _userStatsProvider.TicketsProvider.OnTicketsChanged -= _rouletteScreen.SetupTicketsCount;
            _rouletteScreen.Cleanup();
        }

        public override async UniTask Show()
        {
            _rouletteScreen.SetupTicketsCount(_userStatsProvider.TicketsProvider.Tickets);
            await _rouletteScreen.Show();
        }

        public override async UniTask Hide()
        {
            await _rouletteScreen.Hide();
        }

        private void UpdateWinItems()
        {
            _winItems = new ();
            var inventory = _userStatsProvider.InventoryProvider.Inventory;
    
            foreach (var sector in _rouletteConfig.Sectors)
            {
                var availableItems = sector.WinItems
                    .Where(item => !inventory.Skins.Contains(item.Reward.Id) &&
                                   !inventory.Weapons.Contains(item.Reward.Id) &&
                                   !inventory.Equipment.Contains(item.Reward.Id)).ToList();
        
                var sectorWinItems = new List<RewardConfig>();
        
                for (int i = 0; i < sector.WinItemsCount; i++)
                {
                    if (availableItems.Count == 0) break;
            
                    var item = availableItems[Random.Range(0, availableItems.Count)];
                    item.UpdateConfig();
                    availableItems.Remove(item);
            
                    sectorWinItems.Add(item);
                }
        
                _winItems[sector] = sectorWinItems;
            }
            _rouletteScreen.SetupWinItems(_winItems);
        }

        private async void OnSpinButtonPressed()
        {
            if (!_userStatsProvider.TicketsProvider.IsEnough(1))
            {
                await _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_TICKETS);
                return;
            }

            _userStatsProvider.TicketsProvider.ChangeTickets(-1);
            _rouletteScreen.SetBlockSreen(true);
            var angle = await _roulette.SpinRoulette();
            var result = _roulette.GetConfigByAngle(angle);
            Debug.Log($"Angle: {angle} Sector: {result.Name}");
            
            var availableItems = _winItems[result];
            var winItem = availableItems[Random.Range(0, availableItems.Count)];
            _rewardService.AddReward(winItem);
            await _rewardService.ApplyRewardsAsync();
            
            UpdateWinItems();
            
            _rouletteScreen.SetBlockSreen(false);
        }
    }
}