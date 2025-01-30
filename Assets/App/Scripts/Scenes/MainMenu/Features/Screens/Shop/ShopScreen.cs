using System;
using System.Collections.Generic;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop
{
    public class ShopScreen : GameScreen
    {
        public event Action<int> OnTabClicked;

        [SerializeField] private  List<Button> _tabButtons;
        
        [field:SerializeField] public ScrollRect ScrollRect { get;  private set; }
        [field:SerializeField] public List<RectTransform> Sections { get; private set; }

        public override void Initialize()
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                int index = i;
                _tabButtons[i].onClick.AddListener(() => OnTabClicked?.Invoke(index));
            }
        }

        public void SetScrollPosition(float targetX)
        {
            ScrollRect.DOKill();
            ScrollRect.DONormalizedPos(new Vector2(targetX, 0), 0.5f).SetEase(Ease.OutQuad);
        }

        public void HighlightTab(int index)
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                _tabButtons[i].transform.DOScale(i == index ? 1.2f : 1f, 0.3f).SetEase(Ease.OutBack);
            }
        }

        public override void Cleanup()
        {
            foreach (var tabButton in _tabButtons)
            {
                tabButton.onClick.RemoveAllListeners();
            }
        }
    }

    public class ShopScreenPrezenter : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly ShopScreen _view;
        private readonly int _sectionCount;

        public ShopScreenPrezenter(ShopScreen view)
        {
            _view = view;
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
    }
}