using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass;
using Cysharp.Threading.Tasks;
using SettingsProvider = App.Scripts.Features.Settings.SettingsProvider;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    public class TopViewPrezentor : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly TopView _view;
        private readonly SettingsView _settingsView;
        private readonly SettingsProvider _settingsProvider;
        private readonly List<ITopViewElementPrezenter> _prezenters;
        private readonly RouletteScreenPresentrer _rouletteScreenPresenter;
        private readonly BattlePassScreenPrezenter _battlePassScreenPresenter;

        private ITopViewElementPrezenter _activeScreenPresenter;
        
        public TopViewPrezentor(TopView view,
            SettingsView settingsView,
            SettingsProvider settingsProvider,
            List<ITopViewElementPrezenter> prezenters,
            RouletteScreenPresentrer rouletteScreenPresenter,
            BattlePassScreenPrezenter battlePassScreenPresenter)
        {
            _view = view;
            _settingsView = settingsView;
            _settingsProvider = settingsProvider;
            _prezenters = prezenters;
            _rouletteScreenPresenter = rouletteScreenPresenter;
            _battlePassScreenPresenter = battlePassScreenPresenter;
            _activeScreenPresenter = prezenters[0];
        }

        public override void Initialize()
        {
            _view.Initialize();
            _settingsView.Initialize(_settingsProvider);

            _view.OnSettingsClicked += SettingsClicked;
            _view.OnCloseClicked += OnCloseClicked;
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

        private async void OnCloseClicked()
        {
            if (_activeScreenPresenter!= null)
            {
                await _activeScreenPresenter.Hide();
            }
            _view.SetLastToggle();
        }

        private async void OnToggleClicked(int index)
        {
            await _rouletteScreenPresenter.Hide();
            await _battlePassScreenPresenter.Hide();
            if (_activeScreenPresenter!= null)
            {
                await _activeScreenPresenter.Hide();
            }
            _activeScreenPresenter = _prezenters[index];
            await _activeScreenPresenter.Show();
        }
    }
}