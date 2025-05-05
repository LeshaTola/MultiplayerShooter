using System;
using System.Collections.Generic;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Currency
{
    public class CurrencySection: MonoBehaviour , IInitializable
    {
        private const string SALE_FLAG = "SALE";
        [SerializeField] private List<CurrencyWithSale> _currencies;

        public void Initialize()
        {
            foreach (var currency in _currencies)
            {
                currency.Initialize();
            }
            
            int saleValue = 0;
            Debug.Log($"Flag is {SALE_FLAG}");
            if (!YG2.TryGetFlag(SALE_FLAG, out var sale))
            {
                Debug.Log($"{SALE_FLAG} IS NULL");
            }
            else
            {
                saleValue = int.Parse(sale);
                Debug.Log($"{SALE_FLAG} IS {saleValue}");
            }
            
            if (saleValue > 0)
            {
                foreach (var currency in _currencies)
                {
                    currency.SetSale(saleValue);
                }
            }
            else
            {
                foreach (var currency in _currencies)
                {
                    currency.RemoveSale();
                }
            }
        }
    }
}