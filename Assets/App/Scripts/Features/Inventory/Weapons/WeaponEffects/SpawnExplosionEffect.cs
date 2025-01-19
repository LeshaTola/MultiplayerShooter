using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class SpawnExplosionEffect : WeaponEffect
    {
        public override event Action<Vector3, float> OnPlayerHit;
        
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private Explosion _template;

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            foreach (var hitValue in hitValues)
            {
                var point = hitValue.Item1;
                
                var explosion = PhotonNetwork.Instantiate(_template.name, point, Quaternion.identity).GetComponent<Explosion>();
                explosion.Setup(_radius, (player, distance) =>
                {
                    if (player.Value != 0 && player.Value <= _damage)
                    {
                        LeaderBoardProvider.Instance.AddKill();
                    }
                    
                    player.RPCSetLasHit(Weapon.Owner.photonView.ViewID, Weapon.Config.Id);
                    player.RPCTakeDamage(_damage);
                    OnPlayerHit?.Invoke(player.transform.position, _damage);
                });

                explosion.RPCExplode();
            }
        }

        public override void Import(IWeaponEffect original)
        {
            var concrete = (SpawnExplosionEffect) original;
            _damage = concrete._damage;
            _radius = concrete._radius;
            _template = concrete._template;
        }
    }
}