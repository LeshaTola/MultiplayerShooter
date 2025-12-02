using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.PromoCodes
{
    public class PromoCodesViewPresenter: IInitializable, ICleanupable
    {
        private readonly PromoCodesView _view;
        private readonly ISoundProvider _soundProvider;
        private readonly PromoCodesProvider _promoCodesProvider;

        public PromoCodesViewPresenter(PromoCodesProvider promoCodesProvider,
            PromoCodesView view, ISoundProvider soundProvider)
        {
            _promoCodesProvider = promoCodesProvider;
            _view = view;
            _soundProvider = soundProvider;
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
            _soundProvider.PlayOneShotSound(_view.BuySound);
            _promoCodesProvider.ApplyPromoCode(promoCode);
        }
    }
}