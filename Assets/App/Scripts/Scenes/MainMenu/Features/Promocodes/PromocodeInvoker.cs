using System.Collections.Generic;
using System.Linq;
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

        private PromocodesProvider _promocodesProvider;

        [Inject]
        public void Construct(PromocodesProvider promocodesProvider)
        {
            _promocodesProvider = promocodesProvider;
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(() => _promocodesProvider.ApplyPromocode(_promocode));
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