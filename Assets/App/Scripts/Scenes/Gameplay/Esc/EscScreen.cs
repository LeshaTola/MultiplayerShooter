using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Esc
{
    public class EscScreenPresenter
    {
        private EscMenuView _escMenuView;
        private SettingsView _settingsView;
        private MouseSensivityProvider _mouseSensivityProvider;
        private IAudioService _audioService;
        private Controller.Controller _playerController;

        public EscScreenPresenter(EscMenuView escMenuView,
            SettingsView settingsView,
            IAudioService audioService, 
            MouseSensivityProvider mouseSensitivityProvider,
            Controller.Controller playerController)
        {
            _escMenuView = escMenuView;
            _settingsView = settingsView;
            _mouseSensivityProvider = mouseSensitivityProvider;
            _audioService = audioService;
            _playerController = playerController;
        }

        public void Initialize()
        {
            _escMenuView.Initialize();
            _settingsView.Initialize(_audioService , _mouseSensivityProvider);

            _settingsView.OnCloseButtonClicked += OpenMenu;
            
            _escMenuView.OnContinueButtonClicked += () =>
            {
                Hide();
                _playerController.IsBusy = false;
            };
            _escMenuView.OnSettingsButtonClicked += OpenSettings;
            _escMenuView.OnExitButtonClicked += ()=> PhotonNetwork.LeaveRoom();
        }

        public void OpenSettings()
        {
            _escMenuView.Hide();
            _settingsView.Show();
        }

        public void OpenMenu()
        {
            _escMenuView.Show();
            _settingsView.Hide();
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
        
    }
}