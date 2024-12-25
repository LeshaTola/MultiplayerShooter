using System;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

namespace App.Scripts.Scenes.Gameplay
{
    public class PostProcessingProvider
    {
        private readonly Vignette _vignette;
        private readonly PostProcessingConfig _postProcessingConfig;

        public PostProcessingProvider(PostProcessingConfig postProcessingConfig, PostProcessVolume postProcessVolume)
        {
            _postProcessingConfig = postProcessingConfig;

            if (postProcessVolume.profile.TryGetSettings(out _vignette))
            {
                _vignette.intensity.value = 0f;
            }
        }

        public void ApplyImmortalEffect()
        {
            _vignette.color = _postProcessingConfig.ImmortalColor;
            FadeIn(_postProcessingConfig.FadeInTime, _postProcessingConfig.FadeValue);
        }

        public void RemoveImmortalEffect()
        {
            FadeOut(_postProcessingConfig.FadeOutTime);
        }
        
        public void ApplyDamageEffect()
        {
            _vignette.color = _postProcessingConfig.DamageColor;
            FadeIn();
        }

        public void ApplyHealEffect()
        {
            _vignette.color = _postProcessingConfig.HealColor;
            FadeIn();
        }

        private void FadeIn(float time, float fadeValue, Action completeAction = null)
        {
            DOTween.To(() => _vignette.intensity.value,
                    x => _vignette.intensity.value = x, fadeValue,
                    time)
                .OnComplete(()=>completeAction?.Invoke());
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