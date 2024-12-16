using System.Collections.Generic;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Esc
{
    public class EscScreenPresenter: IInitializable, ICleanupable
    {
        private EscMenuView _escMenuView;
        private SettingsView _settingsView;
        private MouseSensivityProvider _mouseSensivityProvider;
        private IAudioService _audioService;
        private PlayerController _playerPlayerController;

        public EscScreenPresenter(EscMenuView escMenuView,
            SettingsView settingsView,
            IAudioService audioService, 
            MouseSensivityProvider mouseSensitivityProvider/*,
            PlayerController playerPlayerController*/)
        {
            _escMenuView = escMenuView;
            _settingsView = settingsView;
            _mouseSensivityProvider = mouseSensitivityProvider;
            _audioService = audioService;
            /*_playerPlayerController = playerPlayerController;*/
        }

        public void Initialize()
        {
            _escMenuView.Initialize();
            _settingsView.Initialize(_audioService , _mouseSensivityProvider);

            _settingsView.OnCloseButtonClicked += OpenMenu;
            _escMenuView.OnContinueButtonClicked += Continue;
            _escMenuView.OnSettingsButtonClicked += OpenSettings;
            _escMenuView.OnExitButtonClicked += LeaveRoom;
        }

        public void Cleanup()
        {
            _settingsView.OnCloseButtonClicked -= OpenMenu;
            _escMenuView.OnContinueButtonClicked -= Continue;
            _escMenuView.OnSettingsButtonClicked -= OpenSettings;
            _escMenuView.OnExitButtonClicked -= LeaveRoom;
        }

        public void Show()
        {
            _escMenuView.Show();
        }

        public void Hide()
        {
            _escMenuView.Hide();
            _settingsView.Hide();
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
            // _playerPlayerController.IsBusy = false;
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}