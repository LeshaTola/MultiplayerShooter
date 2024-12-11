using System;
using App.Scripts.Scenes.Gameplay.Stats;
using App.Scripts.Scenes.Gameplay.UI.LeaderBoard;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies
{
    public class RaycastShootStrategy : IShootStrategy
    {
        public event Action<Vector3> OnPlayerHit;
        
        private Camera _camera;
        private Weapon _weapon;
        
        public void Init(Camera camera, Weapon weapon)
        {
            _camera = camera;
            _weapon = weapon;
        }

        public void Shoot()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit))
            {
                _weapon.SpawnImpact(hit.point);
                _weapon.NetworkSetLine(hit.point);
                
                if(hit.transform.gameObject.TryGetComponent(out Health health))
                {
                    if (health.Value <= _weapon.Config.Damage)
                    {
                        LeaderBoardProvider.Instance.AddKill();    
                    }
                    
                    OnPlayerHit?.Invoke(hit.point);
                    health.NetworkTakeDamage(_weapon.Config.Damage);
                    health.RPCSetLasHitPlayer(_weapon.Owner.photonView.ViewID);
                }
            }
            else
            {
                _weapon.NetworkSetLine(_weapon.ShootPoint.position + _weapon.ShootPoint.forward * 100);
            }

            _weapon.NetworkFadeOutLine();
        }
    }
}