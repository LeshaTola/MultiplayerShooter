using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.GameMods.Configs;
using App.Scripts.Features.Match.Maps;
using UnityEngine;

namespace App.Scripts.Features.GameMods.Providers
{
    public class GameModProvider
    {
        private readonly MapsProvider _mapsProvider;

        public List<GameModConfig> GameMods { get; }
        public GameModConfig CurrentGameMod { get; private set; }
        
        public GameModProvider(List<GameModConfig> gameMods, 
            MapsProvider mapsProvider)
        {
            GameMods = gameMods;
            _mapsProvider = mapsProvider;
        }

        public void SetRandomGameMod()
        {
            SetGameMod(GameMods[Random.Range(0, GameMods.Count)]);
        }
        
        public void SetGameMod(string id)
        {
            var gameMod = FindGameMod(id);
            if (gameMod == null)
            {
                return;
            }
            
            SetGameMod(gameMod);
        }

        public void SetGameMod(GameModConfig gameMod)
        {
            _mapsProvider.Config = gameMod.Maps;
            CurrentGameMod = gameMod;
        }

        private GameModConfig FindGameMod(string id)
        {
            return GameMods.FirstOrDefault(x=>x.Name.Equals(id));
        }
    }
}