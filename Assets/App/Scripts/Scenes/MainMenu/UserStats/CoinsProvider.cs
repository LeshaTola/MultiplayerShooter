using UnityEngine;

namespace App.Scripts.Features.PlayerStats
{
    public class CoinsProvider
    {
        
        public int Coins { get; private set; }
        
        public void ChangeCoins(int coins)
        {
            Coins += coins;
            Coins = Mathf.Clamp(Coins, 0, int.MaxValue);
        }

        public bool IsEnough(int coins)
        {
            return Coins >= coins;
        }
    }
    
}