using System.Collections.Generic;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Scenes.Gameplay.Timer;
using UnityEngine;

namespace App.Scripts.Features.Match.Maps
{
    public class MapsProvider
    {
        public MapsConfig Config { get; }
        public Map Map { get; set; }

        public MapsProvider(MapsConfig config)
        {
            Config = config;
            SetRandomMap();
        }

        public void SetRandomMap()
        {
            Map = Config.Maps[Random.Range(0, Config.Maps.Count)].Prefab;
        }
    }
}