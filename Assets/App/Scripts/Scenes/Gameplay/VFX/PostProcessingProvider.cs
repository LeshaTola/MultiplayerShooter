using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace App.Scripts.Scenes.Gameplay
{
    public class PostProcessingProvider
    {
        private readonly PostProcessingConfig _postProcessingConfig;
        private readonly Vignette _vignette;

        public PostProcessingProvider(PostProcessingConfig postProcessingConfig, Volume volume)
        {
            _postProcessingConfig = postProcessingConfig;

            if (volume.profile.TryGet(out _vignette))
            {
                _vignette.intensity.value = 0f;
            }
        }
        
        public void ApplyDamageEffect()
        {
            _vignette.color.value = _postProcessingConfig.DamageColor;
            FadeIn();
        }

        public void ApplyHealEffect()
        {
            _vignette.color.value = _postProcessingConfig.HealColor;
            FadeIn();
        }

        public void ApplyImmortalEffect()
        {
            _vignette.color.value = _postProcessingConfig.ImmortalColor;
            FadeIn(_postProcessingConfig.FadeInTime, _postProcessingConfig.FadeValue);
        }

        public void RemoveImmortalEffect()
        {
            FadeOut(_postProcessingConfig.FadeOutTime);
        }

        private void FadeIn(float time, float fadeValue, Action completeAction = null)
        {
            DOTween.To(() => _vignette.intensity.value,
                    x => _vignette.intensity.value = x, fadeValue,
                    time)
                .OnComplete(() => completeAction?.Invoke());
        }

        private void FadeOut(float time)
        {
            DOTween.To(() => _vignette.intensity.value,
                x => _vignette.intensity.value = x,
                0f, time);
        }

        private void FadeIn()
        {
            FadeIn(_postProcessingConfig.FadeInTime, _postProcessingConfig.FadeValue, FadeOut);
        }

        private void FadeOut()
        {
            FadeOut(_postProcessingConfig.FadeOutTime);
        }
    }
}