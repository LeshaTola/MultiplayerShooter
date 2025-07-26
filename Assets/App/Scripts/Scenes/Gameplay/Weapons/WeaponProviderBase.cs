using System;
using System.Collections.Generic;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public abstract class WeaponProviderBase : MonoBehaviourPun
    {
        public virtual event Action<Weapon> OnWeaponChanged;
        public virtual event Action<int> OnWeaponIndexChanged;
        
        protected int WeaponIndex;
        
        public List<Weapon> Weapons { get; protected set;} = new();
        public Weapon CurrentWeapon { get; protected set; }
    }
}