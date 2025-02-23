using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Image;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
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
        private readonly TutorialConfig _tutorialConfig;
        private readonly ImagePopupRouter _imagePopupRouter;
        private readonly ILocalizationSystem _localizationSystem;

        private ITopViewElementPrezenter _activeScreenPresenter;
        private bool _isSwitching;

        public TopViewPrezentor(TopView view,
            SettingsView settingsView,
            SettingsProvider settingsProvider,
            List<ITopViewElementPrezenter> prezenters,
            RouletteScreenPresentrer rouletteScreenPresenter,
            BattlePassScreenPrezenter battlePassScreenPresenter,
            TutorialConfig tutorialConfig,
            ImagePopupRouter imagePopupRouter,
            ILocalizationSystem localizationSystem)
        {
            _view = view;
            _settingsView = settingsView;
            _settingsProvider = settingsProvider;
            _prezenters = prezenters;
            _rouletteScreenPresenter = rouletteScreenPresenter;
            _battlePassScreenPresenter = battlePassScreenPresenter;
            _tutorialConfig = tutorialConfig;
            _imagePopupRouter = imagePopupRouter;
            _localizationSystem = localizationSystem;
            _activeScreenPresenter = prezenters[0];
        }

        public override void Initialize()
        {
            _view.Initialize();
            _settingsView.Initialize(_settingsProvider);

            _view.OnSettingsClicked += SettingsClicked;
            _view.OnTutorClicked += OnTutorClicked;
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

        private async void OnTutorClicked()
        {
            Sprite tutorSprite;
            if (YG2.envir.isDesktop)
            {
                tutorSprite = _localizationSystem.Language == "ru" ? _tutorialConfig.RuTutor : _tutorialConfig.EnTutor;
            }
            else
            {
                tutorSprite = _localizationSystem.Language == "ru"
                    ? _tutorialConfig.RuMobileTutor
                    : _tutorialConfig.EnMobileTutor;
            }

            await _imagePopupRouter.ShowPopup(ConstStrings.INPUT, tutorSprite);
        }

        private async void OnToggleClicked(int index)
        {
            if (_isSwitching) return; // Блокируем повторный вызов
            _isSwitching = true;

            await _rouletteScreenPresenter.Hide();
            await _battlePassScreenPresenter.Hide();

            if (_activeScreenPresenter != null)
            {
                await _activeScreenPresenter.Hide();
            }

            _activeScreenPresenter = _prezenters[index];
            await _activeScreenPresenter.Show();

            _isSwitching = false; // Разблокируем переключение
        }
    }
}