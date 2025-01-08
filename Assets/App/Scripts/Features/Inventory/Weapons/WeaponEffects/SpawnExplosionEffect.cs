using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class SpawnExplosionEffect : WeaponEffect
    {
        public override event Action<Vector3, float> OnPlayerHit;
        
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _radius = 1f;

        private readonly IPool<Explosion> _pool;

        public SpawnExplosionEffect(IPool<Explosion> pool)
        {
            _pool = pool;
        }

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            foreach (var hitValue in hitValues)
            {
                var point = hitValue.Item1;
                
                var explosion = _pool.Get();
                explosion.transform.position = point;
                explosion.Setup(_radius, (player, distance) =>
                {
                    player.RPCSetLasHitPlayer(Weapon.Owner.photonView.ViewID);
                    player.RPCTakeDamage(_damage);
                    if (player.Value <= _damage)
                    {
                        LeaderBoardProvider.Instance.AddKill();
                    }
                    OnPlayerHit?.Invoke(player.transform.position, _damage);
                });

                explosion.Explode();
            }
        }

        public override void Import(IWeaponEffect original)
        {
            var concrete = (SpawnExplosionEffect) original;
            _damage = concrete._damage;
            _radius = concrete._radius;
        }
    }
}