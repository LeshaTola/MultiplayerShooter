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
                _cts.Cancel();
                Weapon.FadeOutLine();
                _isActive = false;
                return;
            }

            if (hitValues.Count < 1)
            {
                return;
            }

            _hitPoint = hitValues[0].Item1;
            Weapon.NetworkSetLine(Weapon.ShootPointProvider.ShotPoint, _hitPoint);
            _isActive = true;
            _cts = new CancellationTokenSource();
            LateUpdateLoop(_cts.Token).Forget();
        }

        private async UniTaskVoid LateUpdateLoop(CancellationToken token)
        {
            await UpdateLinePosition(token);
        }

        private async Task UpdateLinePosition(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    Debug.Log("Update");
                    Weapon.SetLine(Weapon.ShootPointProvider.ShotPoint, _hitPoint);
                    await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    public class GrapplingHookEffect : WeaponEffect
    {
        public override event Action<Vector3, float, bool> OnPlayerHit;

        [SerializeField] private float _overshootYAxis;

        private bool _isActive;

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            if (_isActive)
            {
                _isActive = false;
                return;
            }

            if (hitValues.Count < 1)
            {
                return;
            }

            var hitPoint = hitValues[0].Item1;

            var highestPointOnArc = CalculateHighestPoint(hitPoint);
            Weapon.Owner.JumpToTarget(hitPoint, highestPointOnArc);
            _isActive = true;
        }

        private float CalculateHighestPoint(Vector3 hitPoint)
        {
            var lowestPoint = new Vector3(Weapon.transform.position.x,
                    Weapon.transform.position.y - 1f,
                    Weapon.transform.position.z);

            float grapplePointRelativeYPos = hitPoint.y - lowestPoint.y;
            float highestPointOnArc = grapplePointRelativeYPos + _overshootYAxis;

            if (grapplePointRelativeYPos < 0) highestPointOnArc = _overshootYAxis;
            return highestPointOnArc;
        }

        public override void Import(IWeaponEffect original)
        {
            var concrete = (GrapplingHookEffect) original;
            _overshootYAxis = concrete._overshootYAxis;
        }
    }
}