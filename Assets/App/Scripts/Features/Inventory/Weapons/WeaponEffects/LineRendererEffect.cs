using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class LineRendererEffect : WeaponEffect
    {
        public override event Action<Vector3, float, bool> OnPlayerHit;

        private bool _isActive;
        private CancellationTokenSource _cts;
        private Vector3 _hitPoint;

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            if (_isActive)
            {
                _isActive = false;
                if (_hitPoint != default)
                {
                    _cts.Cancel();
                    Weapon.FadeOutLine();
                    _hitPoint = default;
                    return;
                }
                else
                {
                    return;
                }
            }
            _isActive = true;

            if (hitValues.Count < 1)
            {
                return;
            }

            _hitPoint = hitValues[0].Item1;
            Weapon.NetworkSetLine(Weapon.ShootPointProvider.ShotPoint, _hitPoint);
            _cts = new CancellationTokenSource();
            LateUpdateLoop(_cts.Token).Forget();
        }

        private async UniTaskVoid LateUpdateLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    Weapon.SetLine(Weapon.ShootPointProvider.ShotPoint, _hitPoint);
                    await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}