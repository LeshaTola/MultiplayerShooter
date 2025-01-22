using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.MainMenu.Roulette
{
    public class Roulette
    {
        private readonly RouletteView _view;
        private readonly RouletteConfig _config;

        private List<float> _sectorAngles;
        private float _currentRotation;

        public Roulette(RouletteView view, RouletteConfig config)
        {
            _view = view;
            _config = config;
        }

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
            await _view.Spin(_config.SpinCount.GetRandom(),angle , _config.SpinDuration, _config.SpinCurve);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.ShowDuration));
            _view.SpinToDefault();
            return angle;
        }

        public SectorConfig GetConfigByAngle(float finalAngle)
        {
            float normalizedAngle = finalAngle % 360f;

            for (int i = 0; i < _sectorAngles.Count; i++)
            {
                float startAngle = i == 0 ? 0 : _sectorAngles[i - 1];
                float endAngle = _sectorAngles[i];

                if (normalizedAngle >= startAngle && normalizedAngle < endAngle)
                {
                    return _config.Sectors[i];
                }
            }
            return _config.Sectors.Last();
        }
    }
}