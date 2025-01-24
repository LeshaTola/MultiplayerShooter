using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public interface IWeaponEffect
    {
        public event Action<Vector3, float, bool> OnPlayerHit;

        public void Effect(List<(Vector3, GameObject)> hitValues);
        public void Initialize(Weapon weapon);
        public void Import(IWeaponEffect original);
    }

    public abstract class WeaponEffect : IWeaponEffect
    {
        public abstract event Action<Vector3, float, bool> OnPlayerHit;

        protected Weapon Weapon;

        public abstract void Effect(List<(Vector3, GameObject)> hitValues);

        public virtual void Initialize(Weapon weapon)
        {
            Weapon = weapon;
        }

        public virtual void Import(IWeaponEffect original)
        {
        }
    }
}