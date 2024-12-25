using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies.RaycastBuckshot
{
    public class RaycastBuckshot : ShootStrategy
    {
        public override event Action<Vector3, float> OnPlayerHit;

        [SerializeField] private int _buckshotCount = 5;

        private Dictionary<Health, (int, Vector3)> _damageables = new();

        private readonly Camera _camera;

        public RaycastBuckshot(Camera camera)
        {
            _camera = camera;
        }

        public override void Shoot()
        {
            base.Shoot();

            for (int i = 0; i < _buckshotCount; i++)
            {
                var recoilRotation = Recoil.GetRecoilRotation(_camera.transform);
                var direction = recoilRotation * _camera.transform.forward;
                
                if (!Physics.Raycast(_camera.transform.position, direction, out RaycastHit hit))
                {
                    return;
                }

                Weapon.SpawnImpact(hit.point);
                if (hit.transform.gameObject.TryGetComponent(out Health health))
                {
                    
                    if (health.IsImortal)
                    {
                        return;
                    }
                    
                    if (_damageables.ContainsKey(health))
                    {
                        _damageables[health] = (Weapon.Config.Damage + _damageables[health].Item1, hit.point);
                    }
                    else
                    {
                        _damageables.Add(health, (Weapon.Config.Damage, hit.point));
                    }
                }
            }

            foreach (var damageable in _damageables)
            {
                damageable.Key.RPCSetLasHitPlayer(Weapon.Owner.photonView.ViewID);
                
                if (damageable.Key.Value <= damageable.Value.Item1)
                {
                    LeaderBoardProvider.Instance.AddKill();
                }
                
                OnPlayerHit?.Invoke(damageable.Value.Item2,damageable.Value.Item1);
                damageable.Key.RPCTakeDamage(damageable.Value.Item1);
            }
            _damageables.Clear();
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            var concrete = (RaycastBuckshot) original;
            _buckshotCount = concrete._buckshotCount;
        }
    }
}