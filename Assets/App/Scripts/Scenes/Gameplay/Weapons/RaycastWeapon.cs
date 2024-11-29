using System;
using App.Scripts.Scenes.Gameplay.Stats;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class Raycast : Weapon
    {
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private ParticleSystem _impactEffect;
        [SerializeField] private LineRenderer _tracerEffect;
        [SerializeField] private Camera _camera;

        public override event Action<int, int> OnAmmoChanged;

        private Color _trialStartColor;

        private void Start()
        {
            _trialStartColor = _tracerEffect.startColor;
            _camera = Camera.main;
        }

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

            /*var flash 
                = PhotonNetwork.Instantiate(_muzzleFlash.name, _shootPoint.position, _shootPoint.rotation);*/

            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit))
            {
                var impact
                    = PhotonNetwork.Instantiate(_impactEffect.name, hit.point, Quaternion.LookRotation(hit.normal));
                photonView.RPC(nameof(SetLine),
                    RpcTarget.All,
                    _shootPoint.position,
                    hit.point);
                
                if(hit.transform.gameObject.TryGetComponent(out Health health))
                {
                    health.NetworkTakeDamage(10);
                }
            }
            else
            {
                photonView.RPC(nameof(SetLine),
                    RpcTarget.All,
                    _shootPoint.position,
                        _shootPoint.position + _shootPoint.forward * 100);
            }

            photonView.RPC(nameof(FadeOutLine), RpcTarget.All);

            AttackAnimation();
            CurrentAmmoCount--;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, _maxAmmoCount);
            StartAttackCooldown();
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
                }, new Color(_trialStartColor.r, _trialStartColor.g, _trialStartColor.b, 0f), _attackCooldown)
                .OnComplete(() =>
                {
                    _tracerEffect.enabled = false;
                    _tracerEffect.startColor = _trialStartColor;
                });
        }

        public override void Reload()
        {
            ReloadAnimation();
            StartReloadCooldown();

            CurrentAmmoCount = _maxAmmoCount;
            OnAmmoChanged?.Invoke(CurrentAmmoCount, _maxAmmoCount);
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