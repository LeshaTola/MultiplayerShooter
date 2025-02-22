using App.Scripts.Modules.MinMaxValue;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Tickets
{
    [CreateAssetMenu(menuName ="Configs/Shop/TicketsSection", fileName = "TicketsSectionConfig")]
    public class TicketsSectionConfig : ScriptableObject
    {
        [field: SerializeField] public MinMaxInt TicketsCount { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
    }
}