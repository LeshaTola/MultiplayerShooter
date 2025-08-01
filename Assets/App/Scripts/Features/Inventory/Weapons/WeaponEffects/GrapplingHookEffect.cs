﻿using System;
using System.Collections.Generic;
using System.Threading;
using App.Scripts.Modules.MinMaxValue;
using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Player;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.WeaponEffects
{
    public class GrapplingHookEffect : WeaponEffect
    {
        private const float DISTANCE = 0.5f;

        public override event Action<Vector3, float, bool> OnPlayerHit;

        [SerializeField] private float _overshootYAxis;
        [SerializeField] private MinMaxFloat _minMaxSpeed;
        [SerializeField] private float _speedMultiplier;
        [SerializeField] [Range(0.4f, 2f)] private float _targetFovMultiplier;
        [SerializeField] private float _fovChangeDuration = 0.3f;
        [SerializeField] private float _offMultiplier;

        private readonly float _defaultFov;
        private readonly CinemachineVirtualCamera _playerCamera;

        private CancellationTokenSource _cts;
        private Vector3 _hitPoint;
        private float _speed;
        private Vector3 _direction;
        private bool _isActive;
        private Tween _fovTween;

        public GrapplingHookEffect(CameraProvider cameraProvider)
        {
            _playerCamera = cameraProvider.GetPlayerCamera();
            _defaultFov = _playerCamera.m_Lens.FieldOfView;
        }

        public override void Effect(List<(Vector3, GameObject)> hitValues)
        {
            if (_isActive)
            {
                _isActive = false;
                if (_hitPoint != default)
                {
                    CancelGrappling();
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

            StartGrappling(hitValues);
        }

        private async UniTaskVoid UpdateLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Yield();
                    var distance = Vector3.Distance(Weapon.Owner.PhotonView.transform.position, _hitPoint);
                    _direction = (_hitPoint - Weapon.Owner.PhotonView.transform.position).normalized;
                    _speed = _minMaxSpeed.Clamp(distance) * _speedMultiplier;
                    Weapon.Owner.Movement.GaranteedMove(_direction * _speed* Time.deltaTime);

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

        public override void Import(IWeaponEffect original)
        {
            var concrete = (GrapplingHookEffect) original;
            _overshootYAxis = concrete._overshootYAxis;
            _minMaxSpeed = concrete._minMaxSpeed;
            _speedMultiplier = concrete._speedMultiplier;
            _offMultiplier = concrete._offMultiplier;
            _targetFovMultiplier = concrete._targetFovMultiplier;
            _fovChangeDuration = concrete._fovChangeDuration;
        }

        private void StartGrappling(List<(Vector3, GameObject)> hitValues)
        {
            Weapon.Owner.Movement.PlayerState = PlayerState.Grappling;
            Weapon.Owner.Movement.Freese();
            _hitPoint = hitValues[0].Item1;

            _fovTween?.Kill();
            _fovTween = DOTween.To(
                () => _playerCamera.m_Lens.FieldOfView,
                x => _playerCamera.m_Lens.FieldOfView = x,
                _defaultFov / _targetFovMultiplier,
                _fovChangeDuration
            );

            _cts = new CancellationTokenSource();
            UpdateLoop(_cts.Token).Forget();
        }

        private void CancelGrappling()
        {
            _fovTween?.Kill();
            _fovTween = DOTween.To(
                () => _playerCamera.m_Lens.FieldOfView,
                x => _playerCamera.m_Lens.FieldOfView = x,
                _defaultFov,
                _fovChangeDuration
            );

            _cts.Cancel();
            _hitPoint = default;
            Weapon.Owner.Movement.PlayerState = PlayerState.Normal;
            Weapon.Owner.Movement.AddForce(_direction * _speed * _offMultiplier);
        }
    }
}