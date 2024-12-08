using App.Scripts.Modules.ObjectPool.MonoObjectPools;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using DG.Tweening;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Hitmark
{
    public class HitService: MonoBehaviour
    {
        [SerializeField] private Image _hitMarkImage;

        [Header("Animation")]
        [SerializeField] private float _scaleValue = 0f;
        [SerializeField] private float _scaleAnimationTime = 0.1f;
        [Space]
        [SerializeField] private float _fadeValue = 0.5f;
        [SerializeField] private Color _fadeColor = Color.white;

        [Space]
        [SerializeField] private float _fadeInTime = 0f;
        [SerializeField] private float _fadeOutTime = 0.2f;
        
        [Header("Pool")]
        [SerializeField] private float _upValue = 3f;
        [SerializeField] private float _upTime = 0.3f;
        [SerializeField] private float _scaleUp = 0.3f;
        [SerializeField] private float _scaleDown = 0.3f;
        [SerializeField] private DamageView _damageView;
        [SerializeField] private Transform _container;

        private IPool<DamageView> _pool;

        public void Initialize()
        {
            _pool = new MonoBehObjectPool<DamageView>(_damageView, 10, _container);
        }
        
        public void Hit(Vector3 hitPoint, float damage)
        {
           HitMarkAnimation();
           DamageViewAnimation(hitPoint, damage);
        }

        private void DamageViewAnimation(Vector3 hitPoint, float damage)
        {
            var damageView = _pool.Get();
            damageView.Setup($"{(int)damage}");
            var sequence = DOTween.Sequence();

            var viewTransform = damageView.transform;
            viewTransform.localScale = Vector3.zero;
            viewTransform.position = hitPoint;
            
            sequence.Append(viewTransform.DOScale(1, _scaleUp));
            sequence.Append(viewTransform.DOLocalMoveY(_upValue, _upTime));
            sequence.Append(viewTransform.DOScale(0, _scaleDown));
            
            sequence.onComplete += () =>
            {
                _pool.Release(damageView);
            };
        }

        private void HitMarkAnimation()
        {
            _hitMarkImage.transform.localScale = Vector3.one;
            _hitMarkImage.color = _fadeColor;
            var sequence = DOTween.Sequence();

            sequence.Append(_hitMarkImage.DOFade(_fadeValue, 0f));
            sequence.Join(_hitMarkImage.transform.DOScale(_scaleValue, _scaleAnimationTime));
            sequence.Append(_hitMarkImage.DOFade(0, _fadeOutTime));
        }
    }
}