using System;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies
{
    public interface IShootStrategy
    {
        public event Action<Vector3> OnPlayerHit;

        public void Initialize(Weapon weapon);
        public void Shoot();

        public void Import(IShootStrategy original);
    }
}