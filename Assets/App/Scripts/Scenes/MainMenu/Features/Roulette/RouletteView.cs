using System;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette
{
    public class RouletteView : MonoBehaviour
    {
        public event Action OnDegreePassed;
        
        [SerializeField] private RectTransform _sectorParent;
        [SerializeField] private Image _sectorPrefab;
        
        public UniTask Spin(int spins, float finalAngle, float spinDuration, AnimationCurve spinCurve, int soundEveryDegrees = 90)
        {
            float targetRotation = 360f * spins + finalAngle;
            float lastSoundAngle = 0f;
            
            return _sectorParent
                .DORotate(new Vector3(0, 0, targetRotation), spinDuration, RotateMode.FastBeyond360)
                .SetEase(spinCurve)
                .OnUpdate(() => {
                    float currentAngle = _sectorParent.eulerAngles.z;
                    float delta = Mathf.Abs(currentAngle - lastSoundAngle);
            
                    if (delta >= soundEveryDegrees)
                    {
                        lastSoundAngle = currentAngle;
                        OnDegreePassed?.Invoke();
                    }
                })
                .ToUniTask();
        }

        public void SpinToDefault()
        {
            _sectorParent.DORotate(new Vector3(0, 0, 0), 0.3f, RotateMode.FastBeyond360);
        }

        public void CreateSector(SectorConfig config, float currentFill)
        {
            var sectorImage = Instantiate(_sectorPrefab, _sectorParent);

            sectorImage.color = config.Color;

            sectorImage.fillAmount = config.Percent;
            sectorImage.fillOrigin = 2;
            sectorImage.fillClockwise = true;

            sectorImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -currentFill * 360f);
        }
    }
}