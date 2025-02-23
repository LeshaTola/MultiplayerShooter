using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Settings;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.Gameplay.StateMachine.States;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Esc
{
    public class EscScreenPresenter: IInitializable, ICleanupable
    {
        private readonly EscMenuView _escMenuView;
        private readonly SettingsView _settingsView;
        private readonly SettingsProvider _settingsProvider;
        private readonly MouseSensivityProvider _mouseSensivityProvider;
        private readonly Modules.StateMachine.StateMachine _stateMachine;
        private readonly IAudioService _audioService;
        private readonly PlayerController _playerController;
        private readonly GameInputProvider _gameInputProvider;

        private bool _isActive;
        
        public EscScreenPresenter(EscMenuView escMenuView,
            SettingsView settingsView,
            SettingsProvider settingsProvider,
            Modules.StateMachine.StateMachine stateMachine,
            PlayerController playerController,
            GameInputProvider gameInputProvider)
        {
            _escMenuView = escMenuView;
            _settingsView = settingsView;
            _settingsProvider = settingsProvider;
            _stateMachine = stateMachine;
            _playerController = playerController;
            _gameInputProvider = gameInputProvider;
        }

        public void Initialize()
        {
            _escMenuView.Initialize();
            _settingsView.Initialize(_settingsProvider);

            _settingsView.OnCloseButtonClicked += OpenMenu;
            _escMenuView.OnContinueButtonClicked += Continue;
            _escMenuView.OnSettingsButtonClicked += OpenSettings;
            _escMenuView.OnExitButtonClicked += LeaveRoom;
            _gameInputProvider.OnPause += OnPausePreformed;

        }

        public void Cleanup()
        {
            _settingsView.OnCloseButtonClicked -= OpenMenu;
            _escMenuView.OnContinueButtonClicked -= Continue;
            _escMenuView.OnSettingsButtonClicked -= OpenSettings;
            _escMenuView.OnExitButtonClicked -= LeaveRoom;
            _gameInputProvider.OnPause -= OnPausePreformed;
        }

        public void Show()
        {
            _escMenuView.Show();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Hide()
        {
            _escMenuView.Hide();
            _settingsView.Hide();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OpenSettings()
        {
            _escMenuView.Hide();
            _settingsView.Show();
        }

        private void OpenMenu()
        {
            _escMenuView.Show();
            _settingsView.Hide();
        }

        private void Continue()
        {
            Hide();
            _playerController.IsBusy = false;
            _isActive = false;
        }

        private async void LeaveRoom()
        {
            await _stateMachine.ChangeState<LeaveMatch>();
        }
        
        private void OnPausePreformed()
        {
            if (!_isActive)
            {
                if (_playerController.IsBusy)
                {
                    return;
                }
                Show();

                _playerController.IsBusy = true;
            }
            else
            {
                Hide();
                _playerController.IsBusy = false;
            }
            _isActive = !_isActive;
        }
    }
}