/*using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.WeaponEffects;
using App.Scripts.Features.Inventory.Weapons.ShootingRecoil;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies.Raycast
{
    public class RaycastShootStrategy : ShootStrategy
    {
        public override event Action<Vector3, float> OnPlayerHit;
        
        private readonly Camera _camera;

        public RaycastShootStrategy(Camera camera)
        {
            _camera = camera;
        }

        public override void Shoot()
        {
            base.Shoot();

            var recoilRotation = Recoil.GetRecoilRotation(_camera.transform);
            var direction = recoilRotation * _camera.transform.forward;
            
            if (Physics.Raycast(_camera.transform.position, direction , out var hit))
            {
                WeaponEffect.Effect(new List<(Vector3, GameObject)>()
                {
                    (hit.point, hit.collider.gameObject)
                });
            }
            else
            {
                Weapon.NetworkSetLine(Weapon.ShootPoint.position + Weapon.ShootPoint.forward * 100);
            }

            Weapon.NetworkFadeOutLine();
        }
    }
}*/