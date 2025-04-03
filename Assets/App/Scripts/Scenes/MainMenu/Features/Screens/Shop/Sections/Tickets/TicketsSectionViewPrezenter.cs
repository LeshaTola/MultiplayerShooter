using App.Scripts.Features;
using App.Scripts.Features.Rewards;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Tickets
{
    public class TicketsSectionViewPrezenter : IInitializable, ICleanupable
    {
        private readonly TicketsSectionView _view;
        private readonly TicketsSectionConfig _config;
        private readonly UserStatsProvider _userStatsProvider;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly RewardService _rewardService;
        private readonly ISoundProvider _soundProvider;

        private int _ticketsCount = 1;
        private int _totalCost;

        public TicketsSectionViewPrezenter(TicketsSectionView view,
            TicketsSectionConfig config,
            UserStatsProvider userStatsProvider,
            InfoPopupRouter infoPopupRouter,
            RewardService rewardService,
            ISoundProvider soundProvider)
        {
            _view = view;
            _config = config;
            _userStatsProvider = userStatsProvider;
            _infoPopupRouter = infoPopupRouter;
            _rewardService = rewardService;
            _soundProvider = soundProvider;
        }

        public void Initialize()
        {
            _view.Initialise();
            _view.OnBuyButtonPressed += Buy;
            _view.OnMinusButtonPressed += Minus;
            _view.OnPlusButtonPressed += Plus;
            UpdateCount();
        }

        public void Cleanup()
        {
            _view.Cleanup();
        }

        private void Plus()
        {
            _soundProvider.PlaySound(_view.ClickSound);
            _ticketsCount++;
            UpdateCount();
        }

        private void Minus()
        {
            _soundProvider.PlaySound(_view.ClickSound);
            _ticketsCount--;
            UpdateCount();
        }

        private void Buy()
        {
            if (!_userStatsProvider.CoinsProvider.IsEnough(_totalCost))
            {
                _soundProvider.PlaySound(_view.ClickSound);
                _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
                return;
            }
            _soundProvider.PlaySound(_view.BuySound);
            _userStatsProvider.CoinsProvider.ChangeCoins(-_totalCost);
            _config.TicketReward.Count = _ticketsCount;
            _rewardService.AddReward(_config.TicketReward);
            _rewardService.ApplyRewardsAsync().Forget();
            GameAnalytics.NewDesignEvent($"shop:tickets", _ticketsCount);
        }

        private void UpdateCount()
        {
            _view.SetActiveChangeAmountButtons(_ticketsCount < _config.TicketsCount.Max);
            _view.SetActiveChangeAmountButtons(_ticketsCount > _config.TicketsCount.Min, true);

            _totalCost = _ticketsCount * _config.Cost;
            _view.UpdateCount(_ticketsCount);
            _view.UpdateCost(_totalCost);
        }
    }
}