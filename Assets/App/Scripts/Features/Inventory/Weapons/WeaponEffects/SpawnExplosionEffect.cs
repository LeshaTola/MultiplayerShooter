using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Explosions;
using App.Scripts.Features.Rewards;
using App.Scripts.Modules.MinMaxValue;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class SpawnExplosionEffect : WeaponEffect
    {
        public override event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private MinMaxInt _damage;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private Explosion _template;

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            foreach (var hitValue in hitValues)
            {
                var point = hitValue.Item1;
                
                var explosion 
                    = PhotonNetwork.Instantiate(_template.name, point, Quaternion.identity).GetComponent<Explosion>();
                explosion.Setup(_radius, ProcessExplosion(explosion));

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

        private Action<Health, float> ProcessExplosion(Explosion explosion)
        {
            return (player, distance) =>
            {
                if (!CanDamagePlayer(player, distance) 
                    || IsObstructed(explosion.transform.position, player.transform.position, distance))
                {
                    return;
                }
                    
                var isKilled = false;
                var damage = CalculateDamage(distance);
                if (player.Value <= damage)
                {
                    damage = (int)player.Value;
                    if (player.IsPlayer)
                    {
                        LeaderBoardProvider.Instance.AddKill();
                        RewardsProvider.Instance.ApplyKill();
                    }
                        
                    isKilled = true;
                }
                    
                player.RPCSetLasHit(Weapon.Owner.photonView.ViewID, Weapon.Config.Id);
                player.RPCTakeDamage(damage);
                OnPlayerHit?.Invoke(player.transform.position, damage, isKilled);
            };
        }

        private bool CanDamagePlayer(Health player, float distance)
        {
            return player.Value > 0 && distance <= _radius;
        }

        private bool IsObstructed(Vector3 explosionCenter, Vector3 playerPosition, float distance)
        {
            if (Physics.Raycast(explosionCenter, playerPosition - explosionCenter, out var hit, distance))
            {
                return hit.collider.gameObject.TryGetComponent<Health>(out _);
            }
            return false;
        }

        private int CalculateDamage(float distance)
        {
            var t = distance / _radius;
            return Mathf.RoundToInt(Mathf.Lerp(_damage.Max, _damage.Min, t * t));
        }
    }
}