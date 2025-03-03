using System;
using System.Collections.Generic;
using System.Threading;
using App.Scripts.Modules.MinMaxValue;
using App.Scripts.Scenes.Gameplay.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class GrapplingHookEffect : WeaponEffect
    {
        private const float DISTANCE = 1f;

        public override event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private float _overshootYAxis;
        [SerializeField] private MinMaxFloat _minMaxSpeed;
        [SerializeField] private float _speedMultiplier;
        [SerializeField] private float _offMultiplier;

        private bool _isActive;
        private CancellationTokenSource _cts;
        private Vector3 _hitPoint;
        private float _speed;
        private Vector3 _direction;

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            if (_isActive)
            {
                _cts.Cancel();
                _isActive = false;
                Weapon.Owner.PlayerState = PlayerState.Normal;
                Weapon.Owner.AddForce(_direction * _speed* _offMultiplier);
                return;
            }

            if (hitValues.Count < 1)
            {
                return;
            }

            Weapon.Owner.PlayerState = PlayerState.Grappling;
            Weapon.Owner.Freese();
            _hitPoint = hitValues[0].Item1;

            // var highestPointOnArc = CalculateHighestPoint(_hitPoint);
            // Weapon.Owner.JumpToTarget(_hitPoint, highestPointOnArc);
            _isActive = true;
            _cts = new CancellationTokenSource();
            UpdateLoop(_cts.Token).Forget();
        }

        private async UniTaskVoid UpdateLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Yield();
                    var distance = Vector3.Distance(Weapon.Owner.transform.position, _hitPoint);
                    _direction = (_hitPoint - Weapon.Owner.transform.position).normalized;
                    _speed = _minMaxSpeed.Clamp(distance) * _speedMultiplier;
                    Weapon.Owner.Controller.Move( _direction * _speed * Time.deltaTime);
                    
                    if (distance <= DISTANCE)
                    {
                        _cts.Cancel();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
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
            _minMaxSpeed = concrete._minMaxSpeed;
            _speedMultiplier = concrete._speedMultiplier;
            _offMultiplier = concrete._offMultiplier;
        }
    }
}