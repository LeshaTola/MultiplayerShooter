﻿using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.StateMachines.States;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Screens.MainScreen
{
    public class MainScreenPresenter : GameScreenPresenter
    {
        private readonly MainScreen _screen;
        private readonly StateMachine _stateMachine;

        public MainScreenPresenter(
            MainScreen screen,
            StateMachine stateMachine)
        {
            _screen = screen;
            _stateMachine = stateMachine;
        }

        public override void Initialize()
        {
            _screen.PlayButtonAction += OnPlayButtonAction;
            _screen.Initialize();
        }

        public override void Cleanup()
        {
            _screen.PlayButtonAction -= OnPlayButtonAction;
            
            _screen.Cleanup();
        }

        public override async UniTask Show()
        {
            await _screen.Show();
        }

        public override async UniTask Hide()
        {
            await _screen.Hide();
        }

        private async void OnPlayButtonAction()
        {
            await _stateMachine.ChangeState<RoomState>();
        }
    }
}