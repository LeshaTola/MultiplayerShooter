using System;
using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies.Zoom
{
    public class ZoomStrategy : IShootStrategy
    {
        private readonly MouseSensivityProvider _sensitivityProvider;
        public event Action<Vector3> OnPlayerHit;

        [SerializeField] private float _zoomFOV = 32f;
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _zoomSensivity = 0.6f;

        private float _defaultFOV;
        private float _defaultSensitivity;
        private bool _isZooming;

        private Weapon _weapon;
        private readonly CinemachineVirtualCamera _camera;

        private Tween _currentTween;

        public ZoomStrategy(CameraProvider cameraProvider, MouseSensivityProvider sensitivityProvider)
        {
            _sensitivityProvider = sensitivityProvider;
            _camera = cameraProvider.GetPlayerCamera();
            _defaultFOV = _camera.m_Lens.FieldOfView;
        }

        public void Initialize(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void Shoot()
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

        public void Import(IShootStrategy original)
        {
            ZoomStrategy concreteStrategy = (ZoomStrategy) original;
            _zoomFOV = concreteStrategy._zoomFOV;
            _animationDuration = concreteStrategy._animationDuration;
            _zoomSensivity = concreteStrategy._zoomSensivity;
        }
    }
}