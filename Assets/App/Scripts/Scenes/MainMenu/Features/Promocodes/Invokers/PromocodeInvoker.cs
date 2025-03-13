using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.MainMenu.Features.Promocodes.Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Promocodes
{
    public class PromocodeInvoker : MonoBehaviour
    {
        [SerializeField] protected PromocodesDatabase _promocodesDatabase;

        [ValueDropdown(nameof(GetPromocodes))]
        [SerializeField] protected string _promocode;

        [SerializeField] protected Button _button;

        private PromoCodesProvider _promoCodesProvider;

        [Inject]
        public void Construct(PromoCodesProvider promoCodesProvider)
        {
            _promoCodesProvider = promoCodesProvider;
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(() => _promoCodesProvider.ApplyPromoCode(_promocode));
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