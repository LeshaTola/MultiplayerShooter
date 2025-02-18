using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop
{
    public class ShopScreenElementPrezenter : GameScreenPresenter, IInitializable, ICleanupable, ITopViewElementPrezenter
    {
        private readonly ShopScreen _view;
        private readonly UserStatsView _userStatsView;
        private readonly int _sectionCount;

        public ShopScreenElementPrezenter(ShopScreen view, UserStatsView userStatsView)
        {
            _view = view;
            _userStatsView = userStatsView;
            _sectionCount = view.Sections.Count;
            _view.OnTabClicked += ScrollToSection;
        }

        public override void Initialize()
        {
            _view.Initialize();
        }

        public override void Cleanup()
        {
            _view.Cleanup();
        }

        private void ScrollToSection(int index)
        {
            float targetX = (float)index / (_sectionCount - 1);
            _view.SetScrollPosition(targetX);
            _view.HighlightTab(index);
        }

        public void UpdateScrollPosition()
        {
            float pos = _view.ScrollRect.horizontalNormalizedPosition;
            int closestIndex = 0;
            float closestDistance = Mathf.Abs(pos - (float)closestIndex / (_sectionCount - 1));

            for (int i = 1; i < _sectionCount; i++)
            {
                float distance = Mathf.Abs(pos - (float)i / (_sectionCount - 1));
                if (distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }

            _view.HighlightTab(closestIndex);
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