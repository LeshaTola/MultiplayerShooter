using System;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies
{
    public interface IShootStrategy
    {
        public event Action OnPlayerHit;

        public void Init(Camera camera, Weapon weapon);
        public void Shoot();
    }
}