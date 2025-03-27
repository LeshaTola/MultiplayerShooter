using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.HUD.PlayerUI.Provider;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.HitVisualProvider
{
    public class HitService : IInitializable, ICleanupable
    {
        private readonly PlayerUIProvider _playerUIProvider;
        private readonly HitConfig _config;
        private readonly PlayerProvider _playerProvider;
        private readonly IPool<DamageView> _pool;
        private Sequence _sequence;

        public HitService(PlayerUIProvider playerUIProvider,
            HitConfig config,
            PlayerProvider playerProvider,
            IPool<DamageView> pool)
        {
            _playerUIProvider = playerUIProvider;
            _config = config;
            _playerProvider = playerProvider;
            _pool = pool;
        }

        public void Initialize()
        {
            _playerProvider.Player.WeaponProvider.OnPlayerHit += Hit;
        }

        public void Cleanup()
        {
            _playerProvider.Player.WeaponProvider.OnPlayerHit -= Hit;
        }

        private void Hit(Vector3 hitPoint, float damage, bool killed)
        {
            HitMarkAnimation(killed);
            DamageViewAnimation(hitPoint, damage, killed);
        }

        private void DamageViewAnimation(Vector3 hitPoint, float damage, bool killed)
        {
            var damageView = _pool.Get();
            damageView.Setup($"{(int) damage}", killed);

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

        private void HitMarkAnimation(bool killed)
        {
            if (_sequence.IsActive())
            {
                _sequence.Complete();
                _sequence.Kill();
            }

            var hitMarkImage = _playerUIProvider.PlayerView.WeaponView.HitMarkImage;
            
            hitMarkImage.transform.localScale = Vector3.one;
            hitMarkImage.color = killed? _config.KilledColor:_config.MainColor;
            hitMarkImage.DOFade(1, 0f);
            _sequence = DOTween.Sequence();

            _sequence.Append(hitMarkImage.DOFade(_config.FadeValue, 0f));
            _sequence.Join(hitMarkImage.transform.DOScale(_config.ScaleValue, _config.ScaleAnimationTime));
            _sequence.Append(hitMarkImage.DOFade(0, _config.FadeOutTime));
        }
    }
}