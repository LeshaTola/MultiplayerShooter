﻿using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Factory;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Modules.Factories.MonoFactories;
using App.Scripts.Modules.ObjectPool.MonoObjectPools;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Modules.TasksSystem.Factories;
using App.Scripts.Modules.TasksSystem.Services;
using App.Scripts.Scenes.Gameplay.AI;
using App.Scripts.Scenes.Gameplay.Cameras;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Timer;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using App.Scripts.Scenes.Gameplay.Weapons.TargetDetector;
using App.Scripts.Scenes.MainMenu.Features.Inventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using UnityEngine;
using UnityEngine.Rendering;
using YG;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private TimerProvider _timerProvider;

        [Header("HitMark")]
        [SerializeField] private HitConfig _hitConfig;
        
        [Space]
        [SerializeField] private Transform _container;

        [Header("Player")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Player.Player _playerPrefab;

        [Header("PostProcessing")]
        [SerializeField] private PostProcessingConfig _postProcessingConfig;
        [SerializeField] private Volume _volume;

        [Header("Inventory")]
        [SerializeField] private InventorySlot _slotTemplate;
        [SerializeField] private InventorySlot _mobileSlotTemplate;
        [SerializeField] private Item _itemTemplate;
        [SerializeField] private Item _mobileItemTemplate;
        
        [Header("Other")]
        [SerializeField] private AccrualConfig _accrualConfig;
        [SerializeField] private TargetDetectionConfig _detectionConfig;
        [SerializeField] private SceneNetworkController _sceneNetworkController;
        [SerializeField] private BotController _botController;
        [SerializeField] private BotAI _botAI;

        public override void InstallBindings()
        {
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();
            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();
            Container.Bind<ICleanupService>().To<CleanupService>().AsSingle();

            Container.BindInterfacesAndSelfTo<TimerProvider>().FromInstance(_timerProvider).AsSingle();
            Container.Bind<Camera>().FromInstance(_playerCamera).AsSingle();

            BindInput();
            
            Container.Bind<PlayerProvider>().AsSingle().WithArguments(_playerPrefab);
            Container.Bind<ShootingModeFactory>().AsSingle();
            Container.Bind<ProjectilesFactory>().AsSingle();
            Container.Bind<LeaderBoardProvider>().AsSingle().NonLazy();
            Container.Bind<CameraProvider>().AsSingle();
            Container.Bind<PostProcessingProvider>().AsSingle().WithArguments(_postProcessingConfig, _volume);
            Container.Bind<TargetDetector>().AsSingle().WithArguments(_detectionConfig);

            BindDamageTextPool();
            Container.BindInterfacesAndSelfTo<HitService>().AsSingle().WithArguments(_hitConfig);
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneNetworkController>().FromInstance(_sceneNetworkController).AsSingle();
            Container.Bind<RewardsProvider>().AsSingle().WithArguments(_accrualConfig).NonLazy();

            Container.Bind<BotFactory>().AsSingle().WithArguments(_botAI);
            Container.BindInterfacesAndSelfTo<BotController>().FromInstance(_botController).AsSingle();
            BindTask();


            BindSlotFactory();
            BindItemFactory();
            /*Container.
                Bind<IPool<Explosion>>().
                To<NetworkPool<Explosion>>().
                AsSingle().
                WithArguments(_explosionTemplate, 10, _explosionContainer);*/
        }

        private void BindInput()
        {
            if (YG2.envir.isDesktop)
                Container.Bind<IGameInputProvider>().To<GameInputProvider>().AsSingle();
            else
                Container.Bind<IGameInputProvider>().To<MobileGameInputProvider>().AsSingle();
        }

        private void BindTask()
        {
            Container.BindInterfacesAndSelfTo<TaskService>().AsSingle().NonLazy();
            Container.Bind<TaskFactory>().AsSingle();
            Container.Bind<CompleteActionFactory>().AsSingle();
            Container.Bind<TasksContainerFactory>().AsSingle();
        }


        private void BindItemFactory()
        {
            var item = YG2.envir.isMobile ? _mobileItemTemplate : _itemTemplate; 
            Container.Bind<Modules.Factories.IFactory<Item>>()
                .To<MonoFactory<Item>>()
                .AsSingle()
                .WithArguments(item);
        }

        private void BindSlotFactory()
        {
            var slotTemplate = YG2.envir.isMobile ? _mobileSlotTemplate : _slotTemplate;
            Container
                .Bind<Modules.Factories.IFactory<InventorySlot>>()
                .To<MonoFactory<InventorySlot>>()
                .AsSingle()
                .WithArguments(slotTemplate);
        }

        private void BindDamageTextPool()
        {
            Container.Bind<IPool<DamageView>>().To<MonoBehObjectPool<DamageView>>().AsSingle()
                .WithArguments(_hitConfig.ViewPrefab, 10, _container);
        }
    }
}