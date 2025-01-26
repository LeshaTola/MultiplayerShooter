using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Factory;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Modules.ObjectPool.MonoObjectPools;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Timer;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using App.Scripts.Scenes.MainMenu.Features.Inventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private TimerProvider _timerProvider;
        [SerializeField] private GameConfig _gameConfig;

        [Header("HitMark")]
        [SerializeField] private HitConfig _hitConfig;

        [SerializeField] private Image _hitImage;

        [Space]
        [SerializeField] private Transform _container;

        [Header("Player")]
        [SerializeField] private Camera _playerCamera;

        [SerializeField] private Player.Player _playerPrefab;
        [SerializeField] private List<Transform> _spawnPoints;

        [Header("PostProcessing")]
        [SerializeField] private PostProcessingConfig _postProcessingConfig;

        [SerializeField] private PostProcessVolume _postProcessVolume;

        [Header("Inventory")]
        [SerializeField] private InventorySlot _slotTemplate;

        [SerializeField] private Item _itemTemplate;
        
        [Header("Other")]
        [SerializeField] private AccrualConfig _accrualConfig;

        public override void InstallBindings()
        {
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();
            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();
            Container.Bind<ICleanupService>().To<CleanupService>().AsSingle();

            Container.BindInterfacesAndSelfTo<TimerProvider>().FromInstance(_timerProvider).AsSingle();
            Container.Bind<Camera>().FromInstance(_playerCamera).AsSingle();
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
            Container.Bind<GameInputProvider>().AsSingle();
            Container.Bind<ShootingModeFactory>().AsSingle();
            Container.Bind<ProjectilesFactory>().AsSingle();
            Container.Bind<CameraProvider>().AsSingle();
            Container.Bind<LeaderBoardProvider>().AsSingle().NonLazy();
            Container.Bind<PlayerProvider>().AsSingle().WithArguments(_spawnPoints, _playerPrefab);
            Container.Bind<PostProcessingProvider>().AsSingle()
                .WithArguments(_postProcessingConfig, _postProcessVolume);

            BindDamageTextPool();
            Container.BindInterfacesAndSelfTo<HitService>().AsSingle().WithArguments(_hitConfig, _hitImage);
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.Bind<RewardProvider>().AsSingle().WithArguments(_accrualConfig).NonLazy();

            BindSlotFactory();
            BindItemFactory();
            /*Container.
                Bind<IPool<Explosion>>().
                To<NetworkPool<Explosion>>().
                AsSingle().
                WithArguments(_explosionTemplate, 10, _explosionContainer);*/
        }


        private void BindItemFactory()
        {
            Container.Bind<Modules.Factories.IFactory<Item>>()
                .To<MonoFactory<Item>>()
                .AsSingle()
                .WithArguments(_itemTemplate);
        }

        private void BindSlotFactory()
        {
            Container
                .Bind<Modules.Factories.IFactory<InventorySlot>>()
                .To<MonoFactory<InventorySlot>>()
                .AsSingle()
                .WithArguments(_slotTemplate);
        }

        private void BindDamageTextPool()
        {
            Container.Bind<IPool<DamageView>>().To<MonoBehObjectPool<DamageView>>().AsSingle()
                .WithArguments(_hitConfig.ViewPrefab, 10, _container);
        }
    }
}