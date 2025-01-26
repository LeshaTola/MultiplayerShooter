using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class StandartWeaponEffect : WeaponEffect
    {
        public override event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private int _damage = 10;

        private readonly Dictionary<Health, (int, Vector3)> _damageables = new();

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            foreach (var hitValue in hitValues)
            {
                var point = hitValue.Item1;
                var hitObject = hitValue.Item2;
                Weapon.SpawnImpact(point);

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
                    return;
                }
                damageable.Key.RPCSetLasHit(Weapon.Owner.photonView.ViewID, Weapon.Config.Id);

                var damage = damageable.Value.Item1;
                var isKilled = false;
                if (damageable.Key.Value <= damageable.Value.Item1)
                {
                    damage = (int)damageable.Key.Value;
                    if (damageable.Key.IsPlayer)
                    {
                        LeaderBoardProvider.Instance.AddKill();
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