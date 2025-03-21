﻿using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Zoom
{
    public class ZoomStrategy : ShootStrategy
    {
        private readonly MouseSensivityProvider _sensitivityProvider;

        [SerializeField] private float _zoomFOV = 32f;
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _zoomSensivity = 0.6f;

        private bool _isZooming;
        private float _defaultSensitivity;

        private readonly float _defaultFOV;
        private readonly CinemachineVirtualCamera _camera;

        private Tween _currentTween;

        public ZoomStrategy(Camera camera,
            CameraProvider cameraProvider,
            MouseSensivityProvider sensitivityProvider) : base(camera)
        {
            _sensitivityProvider = sensitivityProvider;
            _camera = cameraProvider.GetPlayerCamera();
            _defaultFOV = _camera.m_Lens.FieldOfView;
        }


        public override void Shoot()
        {
            _currentTween?.Kill();

            float targetFOV;
            if (_isZooming)
            {
                targetFOV = _defaultFOV;
                _sensitivityProvider.SensivityNormalized = _defaultSensitivity;
            }
            else
            {
                _defaultSensitivity = _sensitivityProvider.SensivityNormalized;
                _sensitivityProvider.SensivityNormalized = _zoomFOV/_defaultFOV * _defaultSensitivity * _zoomSensivity;
                targetFOV = _zoomFOV;
            }
            
            _currentTween = DOVirtual.Float(_camera.m_Lens.FieldOfView, targetFOV, _animationDuration,
                (value) => { _camera.m_Lens.FieldOfView = value; }).SetEase(Ease.OutSine);
            
            _isZooming = !_isZooming;
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);
            
            ZoomStrategy concreteStrategy = (ZoomStrategy) original;
            _zoomFOV = concreteStrategy._zoomFOV;
            _animationDuration = concreteStrategy._animationDuration;
            _zoomSensivity = concreteStrategy._zoomSensivity;
        }
    }
}