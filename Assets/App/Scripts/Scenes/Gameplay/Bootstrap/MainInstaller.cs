﻿using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Modules.ObjectPool.MonoObjectPools;
using App.Scripts.Modules.ObjectPool.Pools;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.HitVisualProvider;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Timer;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Bootstrap
{
    public class MainInstaller:MonoInstaller
    {
        [SerializeField] private TimerProvider _timerProvider;
        
        [Header("HitMark")]
        [SerializeField] private HitConfig _hitConfig;
        [SerializeField] private Image _hitImage;

        [Space]
        [SerializeField] private Transform _container;
        
        [Header("Player")]
        [SerializeField] private Player.Player _playerPrefab;
        [SerializeField] private List<Transform> _spawnPoints;

        [Header("PostProcessing")]
        [SerializeField] private PostProcessingConfig _postProcessingConfig;
        [SerializeField] private PostProcessVolume _postProcessVolume;

        [Header("Settings")]
        [SerializeField] private MouseSensivityConfig _mouseSensivityConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();
            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();
            Container.Bind<ICleanupService>().To<CleanupService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<TimerProvider>().FromInstance(_timerProvider).AsSingle();
            Container.Bind<GameInputProvider>().AsSingle();
            Container.Bind<LeaderBoardProvider>().AsSingle().NonLazy();
            Container.Bind<MouseSensivityProvider>().AsSingle().WithArguments(_mouseSensivityConfig);
            Container.Bind<PlayerProvider>().AsSingle().WithArguments(_spawnPoints,_playerPrefab);
            Container.Bind<PostProcessingProvider>().AsSingle().WithArguments(_postProcessingConfig,_postProcessVolume);

            BindDamageTextPool();
            Container.BindInterfacesAndSelfTo<HitService>().AsSingle().WithArguments(_hitConfig, _hitImage);
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }

        private void BindDamageTextPool()
        {
            Container.Bind<IPool<DamageView>>().To<MonoBehObjectPool<DamageView>>().AsSingle()
                .WithArguments(_hitConfig.ViewPrefab, 10, _container);
        }
    }
}