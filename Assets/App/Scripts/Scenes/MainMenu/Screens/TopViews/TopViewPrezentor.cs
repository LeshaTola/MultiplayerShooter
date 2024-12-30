using System;
using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Screens.TopViews
{
    public class TopViewPrezentor : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly TopView _view;
        private readonly SettingsView _settingsView;
        private readonly IAudioService _audioService;
        private readonly StateMachine _stateMachine;
        private readonly MouseSensivityProvider _mouseSensitivityProvider;
        private readonly List<Type> _states;
        
        public TopViewPrezentor(TopView view,
            SettingsView settingsView,
            IAudioService audioService,
            StateMachine stateMachine,
            List<Type> states,
            MouseSensivityProvider mouseSensitivityProvider)
        {
            _view = view;
            _settingsView = settingsView;
            _audioService = audioService;
            _stateMachine = stateMachine;
            _states = states;
            _mouseSensitivityProvider = mouseSensitivityProvider;
        }

        public override void Initialize()
        {
            _view.Initialize();
            _settingsView.Initialize(_audioService, _mouseSensitivityProvider);

            _view.OnSettingsClicked += SettingsClicked;
            _view.OnToggleClicked += OnToggleClicked;
            _settingsView.OnCloseButtonClicked += OnCloseSettingsButtonClicked;
        }

        public override void Cleanup()
        {
            _view.Cleanup();

            _view.OnSettingsClicked -= SettingsClicked;
            _view.OnToggleClicked -= OnToggleClicked;
            _settingsView.OnCloseButtonClicked -= OnCloseSettingsButtonClicked;
        }

        public override async UniTask Show()
        {
            await _view.Show();
        }

        public override async UniTask Hide()
        {
            await _view.Hide();
        }

        private void OnCloseSettingsButtonClicked()
        {
            _settingsView.Hide();
        }

        private void SettingsClicked()
        {
            _settingsView.Show();
        }

        private void OnToggleClicked(int index)
        {
            _stateMachine.ChangeState(_states[index]);
        }
    }
}