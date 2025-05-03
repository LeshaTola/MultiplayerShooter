using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Features.Inventory;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class MainState : State
    {
        private readonly MainScreenPresenter _mainScreenPresenter;
        private readonly RewardService _rewardService;
        private readonly IUpdateService _updateService;
        private readonly List<RewardConfig> _rewardConfigs;

        public MainState(MainScreenPresenter mainScreenPresenter,
            RewardService rewardService, 
            IUpdateService updateService,
            List<RewardConfig> rewardConfigs)
        {
            _mainScreenPresenter = mainScreenPresenter;
            _rewardService = rewardService;
            _updateService = updateService;
            _rewardConfigs = rewardConfigs;
        }

        public override async UniTask Enter()
        {
            _mainScreenPresenter.Setup();
            await _mainScreenPresenter.Show();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            if (_rewardService.HasAnyReward())
            {
                await _rewardService.ApplyRewardsAsync();
            }
        }

        public override async UniTask Exit()
        {
            await _mainScreenPresenter.Hide();
        }

        public override UniTask Update()
        {
            _updateService.Update();
            return UniTask.CompletedTask;
        }
    }
}