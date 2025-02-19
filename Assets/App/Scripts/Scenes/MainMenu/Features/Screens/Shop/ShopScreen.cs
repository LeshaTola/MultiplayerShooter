using System;
using System.Collections.Generic;
using App.Scripts.Features.Screens;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop
{
    public class ShopScreen : GameScreen
    {
        public event Action<int> OnTabClicked;

        [SerializeField] private  List<Button> _tabButtons;
        
        [field:SerializeField] public ScrollRect ScrollRect { get;  private set; }
        [field:SerializeField] public VerticalLayoutGroup LayoutGroup { get; private set; }
        [field:SerializeField] public List<RectTransform> Sections { get; private set; }

        public override void Initialize()
        {
            for (int i = 0; i < _tabButtons.Count; i++)
            {
                int index = i;
                _tabButtons[i].onClick.AddListener(() => OnTabClicked?.Invoke(index));
            }
        }

        public void SetScrollPosition(float target)
        {
            ScrollRect.DOKill();
            ScrollRect.DONormalizedPos(new Vector2(0, target), 0.5f).SetEase(Ease.OutQuad);
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
}