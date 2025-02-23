using App.Scripts.Features;
using App.Scripts.Features.Rewards;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using Cysharp.Threading.Tasks;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Promocodes
{
    public class PromocodesProvider: IInitializable, ICleanupable
    {
        private readonly RewardService _rewardService;
        private readonly PromocodesDatabase _promocodesDatabase;
        private readonly InfoPopupRouter _infoPopupRouter;

        public PromocodesProvider(RewardService rewardService,
            PromocodesDatabase promocodesDatabase,
            InfoPopupRouter infoPopupRouter)
        {
            _rewardService = rewardService;
            _promocodesDatabase = promocodesDatabase;
            _infoPopupRouter = infoPopupRouter;
        }

        public void Initialize()
        {
            YG2.onRewardAdv += ApplyPromocode;
        }

        public void Cleanup()
        {
            YG2.onRewardAdv -= ApplyPromocode;
        }

        public void ApplyPromocode(string promocode)
        {
            if (!_promocodesDatabase.Promocodes.TryGetValue(promocode, out var promocodeRewards))
            {
                _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, "Такого промокода не существует").Forget();
                return;
            }
            
            _rewardService.AddRewards(promocodeRewards);
            _rewardService.ApplyRewardsAsync().Forget();
        }
    }
}