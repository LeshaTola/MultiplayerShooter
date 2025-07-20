using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Modules.AI.Resolver;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using GameAnalyticsSDK.Setup;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotFactory
    {
        private readonly DiContainer _diContainer;
        private readonly BotAI _botPrefab;
        private readonly MapsProvider _mapsProvider;
        private readonly ShootingModeFactory _shootingModeFactory;

        public BotFactory(BotAI botPrefab, MapsProvider mapsProvider, ShootingModeFactory shootingModeFactory, DiContainer diContainer)
        {
            _botPrefab = botPrefab;
            _mapsProvider = mapsProvider;
            _shootingModeFactory = shootingModeFactory;
            _diContainer = diContainer;
        }

        public BotAI GetBot()
        {
            var point = GetSpawnPoint();
            var bot = SpawnBot(point);
            ProjectContext.Instance.Container.InjectGameObject(bot.gameObject);
            // _diContainer.Inject(bot);
            // var weapon = GetWeapon(botConfig, bot);
            // var actionResolver = GetActionResolver(botConfig);

            bot.Initialize();
            return bot;
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