using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette
{
    public class RouletteView : MonoBehaviour
    {
        [SerializeField] private RectTransform _sectorParent;
        [SerializeField] private Image _sectorPrefab;
        

        public UniTask Spin(int spins, float finalAngle, float spinDuration, AnimationCurve spinCurve)
        {
            float targetRotation = 360f * spins + finalAngle;
            return _sectorParent
                .DORotate(new Vector3(0, 0, targetRotation), spinDuration, RotateMode.FastBeyond360)
                .SetEase(spinCurve).ToUniTask();
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