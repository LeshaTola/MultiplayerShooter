using System.Collections.Generic;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Offers
{
    public class OffersView : MonoBehaviour
    {
        [field: SerializeField] public List<PurchaseYG> PurchaseYGs { get; private set; }
        
        public void ShowOnlyValid(List<string> validPromoCodes)
        {
            foreach (var purchaseYG in PurchaseYGs)
            {
                purchaseYG.gameObject.SetActive(validPromoCodes.Contains(purchaseYG.id));
            }
        }
    }
}