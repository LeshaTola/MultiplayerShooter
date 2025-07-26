using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions;
using App.Scripts.Features.Rewards;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class SpawnFireEffect : WeaponEffect
    {
        public override event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _lifeTime = 1f;
        [SerializeField] private float _atackCooldown = 1f;
        [SerializeField] private Fire _template;

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            foreach (var hitValue in hitValues)
            {
                var point = hitValue.Item1;
                
                var fire 
                    = PhotonNetwork.Instantiate(_template.name, point, Quaternion.identity).GetComponent<Fire>();
                fire.Setup(_radius, _lifeTime, _atackCooldown, (player) =>
                {
                    if (player.Value == 0)
                    {
                        return;
                    }
                    
                    var isKilled = false;
                    var damage = _damage;
                    if (player.Value <= _damage)
                    {
                        damage = (int)player.Value;
                        if (player.IsPlayer)
                        {
                            LeaderBoardProvider.Instance.AddKill();
                            RewardsProvider.Instance.ApplyKill();
                        }
                        
                        isKilled = true;
                    }
                    
                    player.RPCSetLasHit(Weapon.Owner.PhotonView.ViewID, Weapon.Config.Id);
                    player.RPCTakeDamage(damage);
                    OnPlayerHit?.Invoke(player.transform.position, _damage, isKilled);
                });

                fire.StartFiring();
            }
        }

        public override void Import(IWeaponEffect original)
        {
            var concrete = (SpawnFireEffect) original;
            _damage = concrete._damage;
            _radius = concrete._radius;
            _lifeTime = concrete._lifeTime;
            _atackCooldown = concrete._atackCooldown;
            _template = concrete._template;
        }
    }
}