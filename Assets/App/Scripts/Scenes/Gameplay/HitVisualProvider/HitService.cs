using DG.Tweening;
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

        public void Hit()
        {
            _hitMarkImage.transform.localScale = Vector3.one;
            _hitMarkImage.color = _fadeColor;
            var sequence = DOTween.Sequence();

            sequence.Append(_hitMarkImage.DOFade(_fadeValue,0f));
            sequence.Join(_hitMarkImage.transform.DOScale(_scaleValue, _scaleAnimationTime));
            sequence.Append(_hitMarkImage.DOFade(0,_fadeOutTime));
        }

    }
}