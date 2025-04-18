using System.Collections.Generic;
using App.Scripts.Features;
using App.Scripts.Features.Screens;
using App.Scripts.Features.Settings;
using App.Scripts.Features.Yandex.Advertisement;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Image;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.TopViews
{
    public class TopViewPresenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly TopView _view;
        private readonly SettingsView _settingsView;
        private readonly SettingsProvider _settingsProvider;
        private readonly List<ITopViewElementPrezenter> _presenters;
        private readonly TutorialConfig _tutorialConfig;
        private readonly ImagePopupRouter _imagePopupRouter;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly ISoundProvider _soundProvider;

        private ITopViewElementPrezenter _activeScreenPresenter;
        private readonly AdvertisementProvider _advertisementProvider;

        public TopViewPresenter(TopView view,
            SettingsView settingsView,
            SettingsProvider settingsProvider,
            List<ITopViewElementPrezenter> presenters,
            TutorialConfig tutorialConfig,
            ImagePopupRouter imagePopupRouter,
            ILocalizationSystem localizationSystem,
            ISoundProvider soundProvider, 
            AdvertisementProvider advertisementProvider)
        {
            _view = view;
            _settingsView = settingsView;
            _settingsProvider = settingsProvider;
            _presenters = presenters;
            _tutorialConfig = tutorialConfig;
            _imagePopupRouter = imagePopupRouter;
            _localizationSystem = localizationSystem;
            _soundProvider = soundProvider;
            _advertisementProvider = advertisementProvider;

            _activeScreenPresenter = presenters[0];
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
            _view.OnTutorClicked -= OnTutorClicked;
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
            _soundProvider.PlaySound(_view.ButtonSound);
            _settingsView.Hide();
        }

        private void SettingsClicked()
        {
            _soundProvider.PlaySound(_view.ButtonSound);
            _settingsView.Show();
        }

        private async void OnTutorClicked()
        {
            _soundProvider.PlaySound(_view.ButtonSound);
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

        private void OnToggleClicked(int index)
        {
            _advertisementProvider.ShowInterstitialAd();

            _activeScreenPresenter?.Hide().Forget();
            _activeScreenPresenter = _presenters[index];
            _activeScreenPresenter.Show().Forget();
            _soundProvider.PlaySound(_view.ToggleSound);
        }
    }
}