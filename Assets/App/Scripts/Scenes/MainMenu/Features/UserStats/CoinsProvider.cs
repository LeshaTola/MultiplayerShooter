using System;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats
{
    public class CoinsProvider
    {
        public event Action<int> OnCoinsChanged;
         
        public int Coins { get; private set; }
        
        public void ChangeCoins(int coins)
        {
            Coins += coins;
            Coins = Mathf.Clamp(Coins, 0, int.MaxValue);
            OnCoinsChanged?.Invoke(Coins);
        }

        public bool IsEnough(int coins)
        {
            return Coins >= coins;
        }
    }
    
}