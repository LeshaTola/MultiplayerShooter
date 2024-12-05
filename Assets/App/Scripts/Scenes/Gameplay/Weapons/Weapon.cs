using System;
using System.Collections;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.Gameplay.Controller;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class Weapon : MonoBehaviourPun
    {
        public event Action<int, int> OnAmmoChanged;
        public event Action OnPlayerHit;

        [Header("Visual")]
        [SerializeField] protected ParticleSystem _muzzleFlash;

        [SerializeField] protected ParticleSystem _impactEffect;
        [SerializeField] protected LineRenderer _tracerEffect;

        [field: Space]
        [field: SerializeField]
        public Transform ShootPoint { get; private set; }

        [Space]
        [SerializeField] private float _trailFadeTime = 0.1f;

        private Color _trialStartColor;
        private bool _isReady = true;

        public WeaponConfig Config { get; private set; }
        public int CurrentAmmoCount { get; private set; }
        public Player Owner { get; private set; }


        public void Initialize(WeaponConfig weaponConfig, Player owner)
        {
            Owner = owner;
            Config = Instantiate(weaponConfig);
            _trialStartColor = _tracerEffect.startColor;
            Config.ShootStrategy.Init(Camera.main, this);
            CurrentAmmoCount = Config.MaxAmmoCount;

            Config.ShootStrategy.OnPlayerHit += () => OnPlayerHit?.Invoke();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _isReady = true;
        }

        private void Update()
        {
            if (Config.ShootingMode.IsShooting)
            {
                PerformAttack();
            }
        }

        public void StartAttack()
        {
            Config.ShootingMode.StartAttack();
        }

        private void PerformAttack()
        {
            if (!_isReady )
            {
                return;
            }

            if (CurrentAmmoCount <= 0)
            {
                Reload();
                return;
            }

            Config.ShootStrategy.Shoot();
            Config.ShootingMode.PerformAttack();

            AttackAnimation();
            CurrentAmmoCount--;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, Config.MaxAmmoCount);
            StartAttackCooldown();
        }

        public void CancelAttack()
        {
            Config.ShootingMode.CancelAttack();
        }

        public void Reload()
        {
            if (!_isReady)
            {
                return;
            }

            ReloadAnimation();
            StartReloadCooldown();
        }

        private void AttackAnimation()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOLocalRotate(new Vector3(-15, 0, 0),
                Config.AttackCooldown / 2));
            sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0),
                Config.AttackCooldown / 2));
        }

        private void ReloadAnimation()
        {
            transform.DOLocalRotate(new Vector3(360, 0, 0),
                Config.ReloadCooldown, RotateMode.FastBeyond360);
        }

        private void StartAttackCooldown()
        {
            StartCoroutine(AttackCooldown());
        }

        private void StartReloadCooldown()
        {
            StartCoroutine(ReloadCooldown());
        }

        private IEnumerator AttackCooldown()
        {
            _isReady = false;
            yield return new WaitForSeconds(Config.AttackCooldown);
            _isReady = true;
        }

        private IEnumerator ReloadCooldown()
        {
            _isReady = false;
            yield return new WaitForSeconds(Config.ReloadCooldown);
            _isReady = true;
            CurrentAmmoCount = Config.MaxAmmoCount;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, Config.MaxAmmoCount);
        }

        public void NetworkFadeOutLine()
        {
            photonView.RPC(nameof(FadeOutLine), RpcTarget.All);
        }

        public void NetworkSetLine(Vector3 endPoint)
        {
            photonView.RPC(nameof(SetLine),
                RpcTarget.All,
                ShootPoint.position,
                endPoint);
        }

        [PunRPC]
        public void SetLine(Vector3 startPos, Vector3 endPos)
        {
            _tracerEffect.SetPosition(0, startPos);
            _tracerEffect.SetPosition(1, endPos);
        }

        [PunRPC]
        public void FadeOutLine()
        {
            _tracerEffect.enabled = true;
            DOTween.To(() => _trialStartColor, x =>
                {
                    var color = x;
                    _tracerEffect.startColor = color;
                    _tracerEffect.endColor = color;
                }, new Color(_trialStartColor.r, _trialStartColor.g, _trialStartColor.b, 0f), _trailFadeTime)
                .OnComplete(() =>
                {
                    _tracerEffect.enabled = false;
                    _tracerEffect.startColor = _trialStartColor;
                });
        }

        public void SpawnImpact(Vector3 position)
        {
            PhotonNetwork.Instantiate(_impactEffect.name, position, Quaternion.identity);
        }

        public void SpawnMuzzleFlash()
        {
            PhotonNetwork.Instantiate(_muzzleFlash.name, ShootPoint.position, ShootPoint.rotation);
        }
    }
}