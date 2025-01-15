using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.StateMachine.States;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.EndGame
{
    public class EndGameViewPresenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly EndGameView _mainView;
        private readonly LeaderBoardView _leaderBoardView;
        private readonly PlayerController _playerController;
        private readonly Modules.StateMachine.StateMachine _stateMachine;

        public EndGameViewPresenter(EndGameView mainView,
            LeaderBoardView leaderBoardView, 
            PlayerController playerController,
            Modules.StateMachine.StateMachine stateMachine)
        {
            _mainView = mainView;
            _leaderBoardView = leaderBoardView;
            _playerController = playerController;
            _stateMachine = stateMachine;
        }

        public override void Initialize()
        {
            _mainView.Initialize();
            _mainView.OnExitButtonPressed += OnExitButtonPressed;
        }

        public override void Cleanup()
        {
            _mainView.Cleanup();
            _mainView.OnExitButtonPressed -= OnExitButtonPressed;
        }

        public override UniTask Show()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            _mainView.Show();
            _leaderBoardView.Show();
            _playerController.IsBusy = true;
            return UniTask.CompletedTask;
        }
        
        public override UniTask Hide()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            _mainView.Hide();
            _leaderBoardView.Hide();
            _playerController.IsBusy = false;
            return UniTask.CompletedTask;
        }

        public void UpdateTimer(int seconds)
        {
            _mainView.UpdateTimer(seconds);
        }

        private async void OnExitButtonPressed()
        {
            await _stateMachine.ChangeState<LeaveMatch>();
        }
    }
}