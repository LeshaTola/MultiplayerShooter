using UnityEngine;

namespace App.Scripts.Features.PlayerStats
{
    public class TicketsProvider
    {
        public int Tickets { get; private set; }

        public void ChangeTickets(int tickets)
        {
            Tickets += tickets;
            Tickets = Mathf.Clamp(Tickets, 0, int.MaxValue);
        }
        
        public bool IsEnough(int tickets)
        {
            return Tickets >= tickets;
        }
    }
}