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

        private void FadeIn()
        {
            DOTween.To(() => _vignette.intensity.value,
                    x => _vignette.intensity.value = x, _postProcessingConfig.FadeValue,
                    _postProcessingConfig.FadeInTime)
                .OnComplete(FadeOut);
        }

        private void FadeOut()
        {
            DOTween.To(() => _vignette.intensity.value,
                x => _vignette.intensity.value = x,
                0f, _postProcessingConfig.FadeOutTime);
        }
    }
}