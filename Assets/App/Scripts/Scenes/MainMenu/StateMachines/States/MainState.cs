using System;
using System.Collections.Generic;
using App.Scripts.Features.Rewards;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class MainState : State
    {
        private readonly MainScreenPresenter _mainScreenPresenter;
        private readonly RewardService _rewardService;

        public MainState(MainScreenPresenter mainScreenPresenter,
            RewardService rewardService)
        {
            _mainScreenPresenter = mainScreenPresenter;
            _rewardService = rewardService;
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
    }
}