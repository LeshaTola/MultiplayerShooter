using System;
using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class StandartWeaponEffect : WeaponEffect
    {
        public override event Action<Vector3, float> OnPlayerHit;
        
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
                damageable.Key.RPCSetLasHitPlayer(Weapon.Owner.photonView.ViewID);

                if (damageable.Key.Value <= damageable.Value.Item1)
                {
                    LeaderBoardProvider.Instance.AddKill();
                }

                OnPlayerHit?.Invoke(damageable.Value.Item2, damageable.Value.Item1);
                damageable.Key.RPCTakeDamage(damageable.Value.Item1);
            }

            _damageables.Clear();
        }
    }
}