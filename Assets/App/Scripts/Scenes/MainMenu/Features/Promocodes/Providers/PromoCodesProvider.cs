using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Factories;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Saves;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Strategies;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers
{
    public class PromoCodesProvider: IInitializable, ICleanupable, ISavable
    {
        public event Action<string> OnPromoCodeApplied;
        
        private readonly PromocodesDatabase _promocodesDatabase;
        private readonly PromocodesFactory _promocodesFactory;
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly IDataProvider<PromocodesSavesData> _promocodesDataProvider;

        public PromocodesSavesData PromoCodesData { get; private set; } = new();

        public PromoCodesProvider(PromocodesDatabase promocodesDatabase,
            PromocodesFactory promocodesFactory,
            InfoPopupRouter infoPopupRouter, 
            IDataProvider<PromocodesSavesData> promocodesDataProvider)
        {
            _promocodesDatabase = promocodesDatabase;
            _promocodesFactory = promocodesFactory;
            _infoPopupRouter = infoPopupRouter;
            _promocodesDataProvider = promocodesDataProvider;
        }

        public void Initialize()
        {
            YG2.onRewardAdv += ApplyPromoCode;
            YG2.onPurchaseSuccess += ApplyPromoCode;
        }

        public void Cleanup()
        {
            YG2.onRewardAdv -= ApplyPromoCode;
            YG2.onPurchaseSuccess -= ApplyPromoCode;
        }

        public void ApplyPromoCode(string promoCode)
        {
            if (!_promocodesDatabase.Promocodes.TryGetValue(promoCode, out var promoCodeActions))
            {
                _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.PROMO_CODE_IS_NOT_EXIST).Forget();
                return;
            }

            var promoCodeData = GetPromoCodeData(promoCode);
            if (promoCodeData != null)
            {
                if (promoCodeActions[0].ExecuteCount != -1 && promoCodeData.Uses >= promoCodeActions[0].ExecuteCount)
                {
                    _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.PROMO_CODE_FINISHED).Forget();
                    return ;
                }
            }
            else
            {
                promoCodeData = new()
                {
                    PromoCode = promoCode,
                    Uses = 0
                };
                PromoCodesData.UsedPromocodes.Add(promoCodeData);
            }

            GameAnalytics.NewDesignEvent($"promo:{promoCodeData.PromoCode.ToLower()}", 1);
            promoCodeData.Uses++;
            OnPromoCodeApplied?.Invoke(promoCode);
            foreach (var action in promoCodeActions)
            {
                var newAction = _promocodesFactory.GetPromocodeAction(action);
                newAction.Execute();
            }

            SaveState();
        }

        public void SaveState()
        {
            _promocodesDataProvider.SaveData(PromoCodesData);
        }

        public void LoadState()
        {
            if (!_promocodesDataProvider.HasData())
            {
                _promocodesDataProvider.SaveData(new());
            }
            PromoCodesData = _promocodesDataProvider.GetData();
            OnPromoCodeApplied?.Invoke("");
        }

        private PromocodeData GetPromoCodeData(string promocode)
        {
            return PromoCodesData.UsedPromocodes.FirstOrDefault(x => x.PromoCode.Equals(promocode));;
        }
    }
}