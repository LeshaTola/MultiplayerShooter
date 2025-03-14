using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Promocodes.Providers;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.PromoCodes
{
    public class PromoCodesViewPresenter: IInitializable, ICleanupable
    {
        private readonly PromoCodesView _view;
        private readonly PromoCodesProvider _promoCodesProvider;

        public PromoCodesViewPresenter(PromoCodesProvider promoCodesProvider, PromoCodesView view)
        {
            _promoCodesProvider = promoCodesProvider;
            _view = view;
        }

        public void Initialize()
        {
            _view.Initialize();
            _view.OnPromoCodeApplied += OnPromoCodeApplied;
        }

        public void Cleanup()
        {
            _view.Cleanup();
        }

        private void OnPromoCodeApplied(string promoCode)
        {
            _promoCodesProvider.ApplyPromoCode(promoCode);
        }
    }
}