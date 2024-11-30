using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies
{
    public interface IShootStrategy
    {
        public void Init(Camera camera, Weapon weapon);
        public void Shoot();
    }
}