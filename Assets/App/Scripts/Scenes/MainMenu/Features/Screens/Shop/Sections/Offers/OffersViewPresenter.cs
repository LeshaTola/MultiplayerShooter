using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Offers
{
    public class OffersViewPresenter: IInitializable, ICleanupable
    {
        private readonly OffersView _offersView;
        private readonly PromoCodesProvider _promoCodesProvider;

        public OffersViewPresenter(OffersView offersView, PromoCodesProvider promoCodesProvider)
        {
            _offersView = offersView;
            _promoCodesProvider = promoCodesProvider;
        }

        public void Initialize()
        {
            UpdateOffers();
            _promoCodesProvider.OnPromoCodeApplied += OnPromoCodeApplied;
        }

        public void Cleanup()
        {
            _promoCodesProvider.OnPromoCodeApplied -= OnPromoCodeApplied;
        }

        private void UpdateOffers()
        {
            var availableOffers = GetValidPromoCodes();
            _offersView.UpdateView(availableOffers);
        }

        private List<string> GetValidPromoCodes()
        {
            var allOffers = _offersView.PurchaseYGs.Select(x => x.id).ToList();
            var usedPromoCodes = _promoCodesProvider.PromoCodesData.UsedPromocodes.Select(p => p.PromoCode).ToHashSet();
            
            return allOffers.Where(offer => !usedPromoCodes.Contains(offer)).ToList();
        }

        private void OnPromoCodeApplied(string obj)
        {
            UpdateOffers();
        }
    }
}