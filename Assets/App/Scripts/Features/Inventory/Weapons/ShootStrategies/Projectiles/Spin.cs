using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles
{
    public class Spin : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction = Vector3.forward;
        [SerializeField] private float _timeOnOneSpin = 1f;

        private void Start()
        {
            RotateContinuously();
        }

        private void RotateContinuously()
        {
            transform.DOLocalRotate(_direction * 360f, _timeOnOneSpin, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}