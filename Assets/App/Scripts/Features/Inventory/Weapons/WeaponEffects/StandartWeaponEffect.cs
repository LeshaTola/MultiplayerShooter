using System;
using System.Collections.Generic;
using App.Scripts.Features.Rewards;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class StandartWeaponEffect : WeaponEffect
    {
        private readonly Camera _camera;
        public override event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private int _damage = 10;

        private readonly Dictionary<Health, (int, Vector3)> _damageables = new();

        public StandartWeaponEffect(Camera camera)
        {
            _camera = camera;
        }

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            var shootPoint = Weapon.ShootPointProvider.ShotPoint;
            if (hitValues.Count == 0)
            {
                Weapon.NetworkSetLine(shootPoint + _camera.transform.forward * 100);
            }
            else
            {
                Weapon.NetworkSetLine(hitValues[0].Item1);
            }
            Weapon.NetworkFadeOutLine();
            
            foreach (var hitValue in hitValues)
            {
                var point = hitValue.Item1;
                var hitObject = hitValue.Item2;
                Weapon.RPCSpawnImpact(point);

                if (!hitObject.TryGetComponent(out Health health) || health.IsImortal)
                {
                    continue;
                }

                if (_damageables.ContainsKey(health))
                {
                    _damageables[health] = (_damage + _damageables[health].Item1, point);
                }
                else
                {
                    _damageables.Add(health, (_damage, point));
                }
            }

            ApplyDamage();
        }

        public override void Import(IWeaponEffect original)
        {
            var concrete = (StandartWeaponEffect) original;
            _damage = concrete._damage;
        }

        private void ApplyDamage()
        {
            foreach (var damageable in _damageables)
            {
                if (damageable.Key.Value == 0)
                {
                    _damageables.Clear();
                    return;
                }
                damageable.Key.RPCSetLasHit(Weapon.Owner.PhotonView.ViewID, Weapon.Config.Id);

                var damage = damageable.Value.Item1;
                var isKilled = false;
                if (damageable.Key.Value <= damageable.Value.Item1)
                {
                    damage = (int)damageable.Key.Value;
                    if (damageable.Key.IsPlayer)
                    {
                        LeaderBoardProvider.Instance.AddKill();
                        RewardsProvider.Instance.ApplyKill();
                    }
                    isKilled = true;
                }

                OnPlayerHit?.Invoke(damageable.Value.Item2, damage, isKilled);
                damageable.Key.RPCTakeDamage(damageable.Value.Item1);
            }

            _damageables.Clear();
        }
    }
}