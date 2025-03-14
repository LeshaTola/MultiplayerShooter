using App.Scripts.Features;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market.Popups;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;

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

        public MarketSectionPrezenter(MarketSectionView view,
            MarketService marketService,
            ILocalizationSystem localizationSystem,
            UserStatsProvider userStatsProvider, 
            InfoPopupRouter infoPopupRouter,
            MarketPopupRouter marketPopupRouter)
        {
            _view = view;
            _marketService = marketService;
            _localizationSystem = localizationSystem;
            _userStatsProvider = userStatsProvider;
            _infoPopupRouter = infoPopupRouter;
            _marketPopupRouter = marketPopupRouter;
        }

        public void Initialzie()
        {
            _view.Initialzie(_localizationSystem);

            _marketService.OnItemsUpdated += _view.UpdateSections;
            _marketService.OnTimerUpdated += _view.UpdateTimer;
            _view.OnItemClicked += OnItemClicked;
            _view.OnUpdateButtonClicked += OnUpdateButtonClicked;
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
            if (!_userStatsProvider.TicketsProvider.IsEnough(1))
            {
                _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_TICKETS).Forget();
                return;
            }
            
            _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.MARKET_IS_UPDATED).Forget();
            _userStatsProvider.TicketsProvider.ChangeTickets(-1);
            _userStatsProvider.SaveState();
            _marketService.UpdateItems();
        }

        private void OnItemClicked(int id)
        {
            _marketPopupRouter.ShowPopup(_marketService.CurrentItems[id]).Forget();
        }
    }
}