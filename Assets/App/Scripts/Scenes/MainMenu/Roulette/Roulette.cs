using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Roulette
{
    public class Roulette
    {
        private RouletteView _view;
        private RouletteConfig _config;

        private List<float> _sectorAngles;
        private float _currentRotation;

        public void GenerateRoulette()
        {
            _sectorAngles = new List<float>();
            float currentFill = 0;

            foreach (var sector in _config.Sectors)
            {
                _view.CreateSector(sector, currentFill);

                float sectorAngle = 360f * sector.Percent;
                _sectorAngles.Add(currentFill * 360f + sectorAngle);

                currentFill += sector.Percent;
            }
        }

        public async UniTask<float> SpinRoulette()
        {
            float angle = Random.Range(0, 360);
            await _view.Spin(_config.SpinCount.GetRandom(), _config.SpinDuration, angle);
            return angle;
        }

        public SectorConfig GetConfigByAngle(float finalAngle)
        {
            float normalizedAngle = (360f - finalAngle) % 360f;

            for (int i = 0; i < _sectorAngles.Count; i++)
            {
                float startAngle = i == 0 ? 0 : _sectorAngles[i - 1];
                float endAngle = _sectorAngles[i];

                if (normalizedAngle >= startAngle && normalizedAngle < endAngle)
                {
                    Debug.Log($"Сектор {i + 1}: Цвет {_config.Sectors[i].Color}");
                    return _config.Sectors[i];
                }
            }
            return _config.Sectors.Last();
        }
    }
}