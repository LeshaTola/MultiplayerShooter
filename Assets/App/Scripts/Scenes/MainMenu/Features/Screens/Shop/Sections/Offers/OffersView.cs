using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Offers
{
    public class OffersView : MonoBehaviour
    {
        [field: SerializeField] public List<PurchaseYG> PurchaseYGs { get; private set; }
        
        public void UpdateView(List<string> validPromoCodes)
        {
            foreach (var purchaseYg in PurchaseYGs)
            {
                purchaseYg.gameObject.SetActive(validPromoCodes.Contains(purchaseYg.id));
            }
        }
    }
}