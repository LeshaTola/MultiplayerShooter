using System;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats
{
    public class TicketsProvider
    {
        public event Action<int> OnTicketsChanged;
        
        public int Tickets { get; private set; }

        public void ChangeTickets(int tickets)
        {
            Tickets += tickets;
            Tickets = Mathf.Clamp(Tickets, 0, int.MaxValue);
            OnTicketsChanged?.Invoke(Tickets);
        }
        
        public bool IsEnough(int tickets)
        {
            return Tickets >= tickets;
        }
    }
}