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
        private readonly List<ITopViewElementPrezenter> _presenters;

        private readonly ISoundProvider _soundProvider;

        private ITopViewElementPrezenter _activeScreenPresenter;
        private readonly AdvertisementProvider _advertisementProvider;

        public TopViewPresenter(TopView view,
            List<ITopViewElementPrezenter> presenters,
            ISoundProvider soundProvider, 
            AdvertisementProvider advertisementProvider)
        {
            _view = view;
            _presenters = presenters;
            _soundProvider = soundProvider;
            _advertisementProvider = advertisementProvider;

            _activeScreenPresenter = presenters[0];
        }

        public override void Initialize()
        {
            _view.Initialize();

            _view.OnToggleClicked += OnToggleClicked;
        }

        public override void Cleanup()
        {
            _view.Cleanup();
            _view.OnToggleClicked -= OnToggleClicked;
        }

        public override async UniTask Show()
        {
            await _view.Show();
        }

        public override async UniTask Hide()
        {
            await _view.Hide();
        }

        private void OnToggleClicked(int index)
        {
            _advertisementProvider.ShowInterstitialAd();

            _activeScreenPresenter?.Hide().Forget();
            _activeScreenPresenter = _presenters[index];
            _activeScreenPresenter.Show().Forget();
            _soundProvider.PlayOneShotSound(AudioKeys.Second_UI);
        }
    }
}