using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class SingleShotWeapon : Weapon
    {
        public override event Action<int, int> OnAmmoChanged;

        [SerializeField] private Bullet bulletTemplate;

        public override void Attack()
        {
            if (!IsReady)
            {
                return;
            }

            if (CurrentAmmoCount <= 0)
            {
                Reload();
                return;
            }

            var bullet = Instantiate(bulletTemplate, _shootPoint.position, Quaternion.identity);
            bullet.Shoot(transform.forward);

            AttackAnimation();

            CurrentAmmoCount--;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, _maxAmmoCount);
            StartAttackCooldown();
        }

        public override void Reload()
        {
            ReloadAnimation();
            StartReloadCooldown();
            CurrentAmmoCount = _maxAmmoCount;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, _maxAmmoCount);
        }

        private void Awake()
        {
            CurrentAmmoCount = _maxAmmoCount;
        }

        private void AttackAnimation()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOLocalRotate(new Vector3(-15, 0, 0), _attackCooldown / 2));
            sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), _attackCooldown / 2));
        }

        private void ReloadAnimation()
        {
            transform.DOLocalRotate(new Vector3(360, 0, 0), _reloadCooldown, RotateMode.FastBeyond360);
        }
    }
}