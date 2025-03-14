using App.Scripts.Features;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Invokers;
using Cysharp.Threading.Tasks;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Offers
{
    public class ReviewOffer : PromocodeInvoker
    {
        protected override void OnEnable()
        {
            _button.onClick.AddListener(ShowReview);
            YG2.onReviewSent += OnReviewSent;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            YG2.onReviewSent -= OnReviewSent;
        }

        private void ShowReview()
        {
            if (!YG2.reviewCanShow)
            {
                ShowReviewError();
                return;
            }

            YG2.ReviewShow();
        }

        private void OnReviewSent(bool success)
        {
            if (!success)
            {
                ShowReviewError();
                return;
            }

            PromoCodesProvider.ApplyPromoCode(_promocode);
        }

        private void ShowReviewError()
        {
            InfoPopupRouter.ShowPopup(ConstStrings.ERROR, ConstStrings.REVIEW_IS_NOT_AVAILABLE).Forget();
        }
    }
}