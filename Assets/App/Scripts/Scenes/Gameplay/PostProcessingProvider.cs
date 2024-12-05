using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace App.Scripts.Scenes.Gameplay
{
    public class PostProcessingProvider: MonoBehaviour
    {
        [Header("General")]
        public PostProcessVolume _postProcessVolume;
        
        [Header("Vignette")]
        [SerializeField] private float _fadeInTime = 0.2f;
        [SerializeField] private float _fadeOutTime= 0.5f;
        [SerializeField] private float _fadeValue;
        [SerializeField] private ColorParameter _damageColor;
        [SerializeField] private ColorParameter _healColor;
        
        private Vignette _vignette;

        private void Awake()
        {
            if (_postProcessVolume.profile.TryGetSettings(out _vignette))
            {
                _vignette.intensity.value = 0f;
            }
        }

        public void ApplyDamageEffect()
        {
            _vignette.color = _damageColor;
            FadeIn();
        }

        public void ApplyHealEffect()
        {
            _vignette.color = _healColor;
            FadeIn();
        }
            
        private void FadeIn()
        {
                DOTween.To(() => _vignette.intensity.value,
                        x => _vignette.intensity.value = x,
                        _fadeValue, _fadeInTime).OnComplete(FadeOut);
        }

        private void FadeOut()
        {
            DOTween.To(() => _vignette.intensity.value,
                x => _vignette.intensity.value = x,
                0f,
                _fadeOutTime);
        }
    }
}