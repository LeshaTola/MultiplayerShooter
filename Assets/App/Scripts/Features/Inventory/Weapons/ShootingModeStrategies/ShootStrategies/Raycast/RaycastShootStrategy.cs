using System;
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
                Weapon.SpawnImpact(hit.point);
                Weapon.NetworkSetLine(hit.point);
                
                if(hit.transform.gameObject.TryGetComponent(out Health health))
                {
                    if (health.IsImortal)
                    {
                        Weapon.NetworkFadeOutLine();
                        return;
                    }
                    health.RPCSetLasHitPlayer(Weapon.Owner.photonView.ViewID);
                    
                    if (health.Value <= Weapon.Config.Damage)
                    {
                        LeaderBoardProvider.Instance.AddKill();
                    }
                    
                    OnPlayerHit?.Invoke(hit.point,Weapon.Config.Damage);
                    health.RPCTakeDamage(Weapon.Config.Damage);
                }
            }
            else
            {
                Weapon.NetworkSetLine(Weapon.ShootPoint.position + Weapon.ShootPoint.forward * 100);
            }

            Weapon.NetworkFadeOutLine();
        }
    }
}