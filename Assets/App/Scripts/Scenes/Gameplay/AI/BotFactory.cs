using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using Module.AI.Resolver;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotFactory
    {
        private readonly BotAI _botPrefab;
        private readonly MapsProvider _mapsProvider;
        private readonly ShootingModeFactory _shootingModeFactory;

        public BotFactory(BotAI botPrefab, MapsProvider mapsProvider, ShootingModeFactory shootingModeFactory)
        {
            _botPrefab = botPrefab;
            _mapsProvider = mapsProvider;
            _shootingModeFactory = shootingModeFactory;
        }

        public BotAI GetBot(BotConfig botConfig)
        {
            var point = GetSpawnPoint();
            var bot = SpawnBot(point);
            var weapon = GetWeapon(botConfig, bot);
            var actionResolver = GetActionResolver(botConfig);

            // bot.Initialize(actionResolver, weapon);
            return bot;
        }

        private static IActionResolver GetActionResolver(BotConfig botConfig)
        {
            IActionResolver actionResolver = new ActionResolver();
            actionResolver.Init(botConfig.Actions);
            return actionResolver;
        }

        private Weapon GetWeapon(BotConfig botConfig, BotAI bot)
        {
            var weaponConfig = botConfig.Weapons[Random.Range(0, botConfig.Weapons.Count)];
            var weaponObject = SpawnWeapon(bot, weaponConfig);
                
            var newConfig = GetNewWeaponConfig(weaponConfig);
            // weaponObject.Initialize(newConfig);
            return weaponObject;
        }

        private static Weapon SpawnWeapon(BotAI bot, WeaponConfig weaponConfig)
        {
            var weaponObject
                = PhotonNetwork.Instantiate(weaponConfig.Prefab.name, bot.WeaponHolder.position, bot.WeaponHolder.rotation)
                    .GetComponent<Weapon>();
            return weaponObject;
        }

        private WeaponConfig GetNewWeaponConfig(WeaponConfig weaponConfig)
        {
            var newConfig = Object.Instantiate(weaponConfig);
            var shootingMode = _shootingModeFactory.GetShootingMode(weaponConfig.ShootingMode);
            var shootingModeAlternative = _shootingModeFactory.GetShootingMode(weaponConfig.ShootingModeAlternative);
            newConfig.Initialize(shootingMode,shootingModeAlternative);
            return newConfig;
        }

        private BotAI SpawnBot(Transform point)
        {
            var botObject = PhotonNetwork.InstantiateRoomObject(_botPrefab.name, point.position, Quaternion.identity);
            var bot = botObject.GetComponent<BotAI>();
            return bot;
        }

        private Transform GetSpawnPoint()
        {
            var spawnPoints = _mapsProvider.CurrentMap.SpawnPoints;
            var point = spawnPoints[Random.Range(0, spawnPoints.Count)];
            return point;
        }
    }
}