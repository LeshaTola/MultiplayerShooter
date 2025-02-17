using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
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
        private readonly List<RewardConfig> _rewardConfigs;

        public MainState(MainScreenPresenter mainScreenPresenter,
            RewardService rewardService, List<RewardConfig> rewardConfigs)
        {
            _mainScreenPresenter = mainScreenPresenter;
            _rewardService = rewardService;
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

        public override async UniTask Update()
        {
            if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.Y))
            {
                _rewardService.ExperienceToAdd = 300;
                _rewardService.AddRewards(_rewardConfigs);
                await _rewardService.ApplyRewardsAsync();
            }
        }
    }
}