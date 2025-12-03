using App.Scripts.Features;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Invokers;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;
using Cysharp.Threading.Tasks;
using RuStore.Review;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Offers
{
    public class RuStoreReviewOffer : PromocodeInvoker
    {
        #if RUSTORE
        protected override void OnEnable()
        {
            _button.onClick.AddListener(ShowReview);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _button.onClick.RemoveListener(ShowReview);
        }

        private void ShowReview()
        {
            RuStoreReviewManager.Instance.RequestReviewFlow(
                onFailure: (error) =>
                {
                    InfoPopupRouter.ShowPopup(ConstStrings.ERROR, ConstStrings.REVIEW_IS_NOT_AVAILABLE).Forget();
                },
                onSuccess: () => {
                    PromoCodesProvider.ApplyPromoCode(_promocode);
                }
            );
        }
        #endif
    }
}