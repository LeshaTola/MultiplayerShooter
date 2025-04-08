using App.Scripts.Features;
using App.Scripts.Features.Yandex.Advertisement;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using Cysharp.Threading.Tasks;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Strategies
{
    public class TurnOffAdPromocode : PromocodeAction
    {
        private readonly AdvertisementProvider _advertisementProvider;
        private readonly InfoPopupRouter _infoPopupRouter;

        public TurnOffAdPromocode(AdvertisementProvider advertisementProvider,
            InfoPopupRouter infoPopupRouter)
        {
            _advertisementProvider = advertisementProvider;
            _infoPopupRouter = infoPopupRouter;
        }

        public override void Execute()
        {
            _advertisementProvider.IsCanShowAd = false;
            _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.AD_IS_OFF).Forget();
            YG2.saves.IsCanShowAd = false;
            YG2.StickyAdActivity(YG2.saves.IsCanShowAd);
        }
    }
}