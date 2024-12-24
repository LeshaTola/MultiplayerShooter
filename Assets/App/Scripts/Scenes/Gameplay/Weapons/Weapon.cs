using System;
using System.Collections;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Weapons.Animations;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class Weapon : MonoBehaviourPun
    {
        public event Action<int, int> OnAmmoChanged;
        public event Action<Vector3> OnPlayerHit;
        public event Action<float> OnReloadStarted;
        public event Action OnReloadFinised;

        [field: Header("Animation")]
        [field: SerializeField]
        public WeaponAnimator Animator { get; private set; }

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
        private bool _isReloading;
        private bool _isLocal;

        public WeaponConfig Config { get; private set; }
        public int CurrentAmmoCount { get; private set; }
        public Player.Player Owner { get; private set; }

        private void Start() //TODO: Is a problem
        {
            _trialStartColor = _tracerEffect.startColor;
        }

        public void Initialize(WeaponConfig weaponConfig)
        {
            Config = weaponConfig;
            _isLocal = true;
            
            Animator.Initialize(this);
            
            Config.ShootingMode.Initialize(this);
            Config.ShootingModeAlternative.Initialize(this);
            
            CurrentAmmoCount = Config.MaxAmmoCount;

            Config.ShootingMode.ShootStrategy.OnPlayerHit += (value) => OnPlayerHit?.Invoke(value);
            Config.ShootingModeAlternative.ShootStrategy.OnPlayerHit += (value) => OnPlayerHit?.Invoke(value);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if (_isReloading)
            {
                OnReloadFinised?.Invoke();
                _isReloading = false;
            }
            _isReady = true;
        }

        private void Update()
        {
            PerformAttack();
        }

        public void StartAttack(bool isAlternative)
        {
            if (!isAlternative)
            {
                Config.ShootingMode.StartAttack();
                return;
            }
            Config.ShootingModeAlternative.StartAttack();
        }

        public void CancelAttack(bool isAlternative)
        {
            if (!isAlternative)
            {
                Config.ShootingMode.CancelAttack();
                return;
            }
            Config.ShootingModeAlternative.CancelAttack();
        }

        private void PerformAttack()
        {
            if (!_isLocal || !_isReady)
            {
                return;
            }
            
            if (CurrentAmmoCount <= 0)
            {
                Reload();
                return;
            }
            
            if (Config.ShootingMode.IsShooting)
            {
                Config.ShootingMode.PerformAttack();
                return;
            }

            if (Config.ShootingModeAlternative.IsShooting)
            {
                Config.ShootingModeAlternative.PerformAttack();
            }
        }

        public void Reload()
        {
            if (!_isReady || CurrentAmmoCount == Config.MaxAmmoCount)
            {
                return;
            }

            Animator.ReloadAnimation();
            StartReloadCooldown();
        }

        public void SetOwner(Player.Player player)
        {
            Owner = player;
        }

        public void StartAttackCooldown(float cooldown)
        {
            StartCoroutine(AttackCooldown(cooldown));
        }

        public void StartReloadCooldown()
        {
            StartCoroutine(ReloadCooldown());
        }

        private IEnumerator AttackCooldown(float cooldown)
        {
            _isReady = false;
            yield return new WaitForSeconds(cooldown);
            _isReady = true;
        }

        private IEnumerator ReloadCooldown()
        {
            OnReloadStarted?.Invoke(Config.ReloadCooldown);
            _isReady = false;
            _isReloading = true;
            yield return new WaitForSeconds(Config.ReloadCooldown);
            _isReady = false;
            _isReady = true;
            CurrentAmmoCount = Config.MaxAmmoCount;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, Config.MaxAmmoCount);
            OnReloadFinised?.Invoke();
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

        public void ChangeAmmoCount(int ammo)
        {
            CurrentAmmoCount += ammo;
            CurrentAmmoCount = Mathf.Clamp(CurrentAmmoCount, 0, Config.MaxAmmoCount);
            OnAmmoChanged?.Invoke(CurrentAmmoCount,Config.MaxAmmoCount);
        }
    }
}