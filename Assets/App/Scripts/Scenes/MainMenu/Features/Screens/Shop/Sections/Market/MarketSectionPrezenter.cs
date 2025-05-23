﻿using System;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market
{
    public class MarketSectionPrezenter
    {
        private readonly MarketSectionView _view;
        private readonly MarketService _marketService;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly MarketPopupRouter _marketPopupRouter;
        private readonly ISoundProvider _soundProvider;

        public MarketSectionPrezenter(MarketSectionView view,
            MarketService marketService,
            ILocalizationSystem localizationSystem,
            UserStatsProvider userStatsProvider, 
            InfoPopupRouter infoPopupRouter,
            MarketPopupRouter marketPopupRouter,
            ISoundProvider soundProvider)
        {
            _view = view;
            _marketService = marketService;
            _localizationSystem = localizationSystem;
            _userStatsProvider = userStatsProvider;
            _infoPopupRouter = infoPopupRouter;
            _marketPopupRouter = marketPopupRouter;
            _soundProvider = soundProvider;
        }

        public void Initialzie()
        {
            _view.Initialzie(_localizationSystem);

            _marketService.OnItemsUpdated += _view.UpdateSections;
            _marketService.OnCurrencyCountUpdated += _view.UpdateCurrencyCount;
            _marketService.OnTimerUpdated += _view.UpdateTimer;
            _view.OnItemClicked += OnItemClicked;
            _view.OnUpdateButtonClicked += OnUpdateButtonClicked;
            _view.OnCurrencyInvoked += ChangeCurrencyCount;
        }

        public void Cleanup()
        {
            _view.Cleanup();

            _marketService.OnItemsUpdated -= _view.UpdateSections;
            _marketService.OnTimerUpdated -= _view.UpdateTimer;
            _view.OnItemClicked -= OnItemClicked;
            _view.OnUpdateButtonClicked -= OnUpdateButtonClicked;
        }

        private void OnUpdateButtonClicked()
        {
            _soundProvider.PlaySound(_view.ClickSound);
            /*if (!_userStatsProvider.TicketsProvider.IsEnough(1))
            {
                _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_TICKETS).Forget();
                return;
            }*/

            YG2.RewardedAdvShow(String.Empty, () =>
            {
                _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.MARKET_IS_UPDATED).Forget();
                _marketService.UpdateItems();
            });
            
            // _userStatsProvider.TicketsProvider.ChangeTickets(-1);
            // _userStatsProvider.SaveState();
        }

        private void OnItemClicked(string id)
        {
            _soundProvider.PlaySound(_view.ClickSound);
            var config
                = _marketService.CurrentWeapons.FirstOrDefault(x => x.Item.Id.Equals(id))
                  ?? _marketService.CurrentSkins.FirstOrDefault(x => x.Item.Id.Equals(id));
            _marketPopupRouter.ShowPopup(config).Forget();
        }

        private void ChangeCurrencyCount()
        {
            _marketService.CurrentCurrencyElementsCount--;
        }
    }
}