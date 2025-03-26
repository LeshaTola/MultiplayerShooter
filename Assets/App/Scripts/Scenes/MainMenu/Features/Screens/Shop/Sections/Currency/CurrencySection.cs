using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Currency
{
    public class CurrencySection: MonoBehaviour
    {
        [SerializeField] private List<CurrencyWithSale> _currencies;

        public void Start()
        {
            var sale = YG2.GetFlag("SALE");
            var saleValue = int.Parse(sale);

            if (saleValue>0)
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