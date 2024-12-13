using App.Scripts.Modules.ObjectPool.MonoObjectPools;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.HitVisualProvider
{
    public class HitService: IInitializable, ICleanupable
    {
        private readonly Image _hitMarkImage;
        private readonly HitConfig _config;
        private readonly PlayerProvider _playerProvider;
        private readonly IPool<DamageView> _pool;

        public HitService(Image hitMarkImage, HitConfig config, Transform container, PlayerProvider playerProvider)
        {
            _hitMarkImage = hitMarkImage;
            _config = config;
            _playerProvider = playerProvider;
            _pool = new MonoBehObjectPool<DamageView>(_config.ViewPrefab, 10, container); //TODO: Move to installer
        }

        public void Initialize()
        {
            _playerProvider.Player.WeaponProvider.OnPlayerHit += Hit;
        }

        public void Cleanup()
        {
            _playerProvider.Player.WeaponProvider.OnPlayerHit -= Hit;
        }

        private void Hit(Vector3 hitPoint, float damage)
        {
            HitMarkAnimation();
            DamageViewAnimation(hitPoint, damage);
        }

        private void DamageViewAnimation(Vector3 hitPoint, float damage)
        {
            var damageView = _pool.Get();
            damageView.Setup($"{(int) damage}");

            damageView.transform.position = hitPoint;
            var viewTransform = damageView.ContentTransform;
            viewTransform.localScale = Vector3.zero;
            viewTransform.localPosition = new Vector3(0, 0, viewTransform.localPosition.z);

            var sequence = DOTween.Sequence();
            sequence.Append(viewTransform.DOScale(1, _config.ScaleUp));
            sequence.Append(viewTransform.DOLocalMoveY(_config.UpValue, _config.UpTime));
            sequence.Append(viewTransform.DOScale(0, _config.ScaleDown));

            sequence.onComplete += () => { _pool.Release(damageView); };
        }

        private void HitMarkAnimation()
        {
            _hitMarkImage.transform.localScale = Vector3.one;
            _hitMarkImage.color = _config.FadeColor;
            var sequence = DOTween.Sequence();

            sequence.Append(_hitMarkImage.DOFade(_config.FadeValue, 0f));
            sequence.Join(_hitMarkImage.transform.DOScale(_config.ScaleValue, _config.ScaleAnimationTime));
            sequence.Append(_hitMarkImage.DOFade(0, _config.FadeOutTime));
        }
    }
}