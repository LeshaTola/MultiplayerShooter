using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop
{
    public class ShopScreenElementPrezenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private readonly ShopScreen _view;
        private readonly UserStatsView _userStatsView;
        private readonly MarketSectionPrezenter _marketSectionPrezenter;
        private readonly ISoundProvider _soundProvider;

        private readonly List<float> _startNormalizedPositions = new();

        public ShopScreenElementPrezenter(ShopScreen view,
            UserStatsView userStatsView,
            MarketSectionPrezenter marketSectionPrezenter,
            ISoundProvider soundProvider)
        {
            _view = view;
            _userStatsView = userStatsView;
            _marketSectionPrezenter = marketSectionPrezenter;
            _soundProvider = soundProvider;

            _view.OnTabClicked += ScrollToSection;
        }

        public override void Initialize()
        {
            _view.Initialize();
            _marketSectionPrezenter.Initialzie();
                
            float value = 1;
            _view.Show();
            var contentHeight = _view.ScrollRect.content.rect.height;
            var viewportHeightNormalized = _view.ScrollRect.viewport.rect.height/contentHeight/2;
            _view.Hide();
            
            _startNormalizedPositions.Add(value);
            for (int i = 0; i < _view.Sections.Count-1; i++)
            {
                float sectionHeight = _view.Sections[i].rect.height;
                value -= sectionHeight / contentHeight;
                value -= viewportHeightNormalized;
                _startNormalizedPositions.Add(Mathf.Clamp01(value));
            }
        }

        public override void Cleanup()
        {
            _view.Cleanup();
            _marketSectionPrezenter.Cleanup();
        }

        private void ScrollToSection(int index)
        {
            _soundProvider.PlaySound(_view.ClickSound);
            _view.SetScrollPosition(_startNormalizedPositions[index]);
            _view.HighlightTab(index);
        }
        
        public override async UniTask Show()
        {
            await _userStatsView.Show();
            await _view.Show();
        }

        public override async UniTask Hide()
        {
            await _userStatsView.Hide();
            await _view.Hide();
        }
    }
}