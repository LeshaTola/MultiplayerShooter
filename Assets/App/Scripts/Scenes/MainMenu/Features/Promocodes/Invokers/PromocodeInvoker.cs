using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Invokers
{
    public class PromocodeInvoker : MonoBehaviour
    {
        [SerializeField] protected PromocodesDatabase _promocodesDatabase;

        [ValueDropdown(nameof(GetPromocodes))]
        [SerializeField] protected string _promocode;

        [SerializeField] protected Button _button;

        protected PromoCodesProvider PromoCodesProvider;
        protected InfoPopupRouter InfoPopupRouter;

        [Inject]
        public void Construct(PromoCodesProvider promoCodesProvider, InfoPopupRouter infoPopupRouter)
        {
            PromoCodesProvider = promoCodesProvider;
            InfoPopupRouter = infoPopupRouter;
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(() => PromoCodesProvider.ApplyPromoCode(_promocode));
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private List<string> GetPromocodes()
        {
            return _promocodesDatabase.Promocodes.Keys.ToList();
        }
    }
}