using System;
using App.Scripts.Scenes.Gameplay.Cameras;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.ShootStrategies.Zoom
{
    public class ZoomStrategy : IShootStrategy
    {
        public event Action<Vector3> OnPlayerHit;

        [SerializeField] private float _zoomFOV = 32f;
        [SerializeField] private float _animationDuration = 0.3f;

        private float _defaultFOV;
        private bool _isZooming;

        private Weapon _weapon;
        private readonly CinemachineVirtualCamera _camera;

        private Tween _currentTween;

        public ZoomStrategy(CameraProvider cameraProvider)
        {
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

            float targetFOV = _isZooming ? _defaultFOV : _zoomFOV;
            _currentTween = DOVirtual.Float(_camera.m_Lens.FieldOfView, targetFOV, _animationDuration,
                (value) => { _camera.m_Lens.FieldOfView = value; }).SetEase(Ease.OutSine);

            _isZooming = !_isZooming;
        }

        public void Import(IShootStrategy original)
        {
            ZoomStrategy concreteStrategy = (ZoomStrategy) original;
            _zoomFOV = concreteStrategy._zoomFOV;
            _animationDuration = concreteStrategy._animationDuration;
        }
    }
}