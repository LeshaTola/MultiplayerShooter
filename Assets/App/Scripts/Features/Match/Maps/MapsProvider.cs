using System.Collections.Generic;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Scenes.Gameplay.Timer;
using UnityEngine;

namespace App.Scripts.Features.Match.Maps
{
    public class MapsProvider
    {
        public MapsConfig Config { get; set;}
        
        public Map Map { get; set; }
        public MapConfig MapConfig { get; set; }
        
        public Map CurrentMap { get; set; }
        

        public void SetRandomMap()
        {
            MapConfig = Config.Maps[Random.Range(0, Config.Maps.Count)];
            Map = MapConfig.Prefab;
        }
    }
}