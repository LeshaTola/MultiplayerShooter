using System.Collections.Generic;
using App.Scripts.Features.Commands;
using App.Scripts.Features.GameMods.Configs;
using App.Scripts.Features.GameMods.Providers;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Features.Rewards;
using App.Scripts.Features.SceneTransitions;
using App.Scripts.Features.Settings;
using App.Scripts.Features.StateMachines.States;
using App.Scripts.Features.UserStats.Rank;
using App.Scripts.Features.UserStats.Rank.Configs;
using App.Scripts.Features.Yandex.Advertisement;
using App.Scripts.Features.Yandex.Saves;
using App.Scripts.Modules.Commands.Provider;
using App.Scripts.Modules.FileProvider;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Configs;
using App.Scripts.Modules.Localization.Data;
using App.Scripts.Modules.Localization.Keys;
using App.Scripts.Modules.Localization.Parsers;
using App.Scripts.Modules.Resolutions;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Modules.Sounds.Services;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Modules.TasksSystem.Configs;
using App.Scripts.Modules.TasksSystem.Factories;
using App.Scripts.Modules.TasksSystem.Providers;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Factories;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Saves;
using App.Scripts.Scenes.MainMenu.Features.RoomsProviders;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using TNRD;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using YG;
using Zenject;

namespace App.Scripts.Features.Bootstrap
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private LocalizationDatabase _localizationDatabase;
        [SerializeField] private string _language;

        [SerializeField] private SerializableInterface<ISceneTransition> _sceneTransition;

        [SerializeField] private ConnectionProvider _connectionProvider;

        [Header("Game")]
        [SerializeField] private List<GameModConfig> _gameModsConfig;
        [SerializeField] private RoomsProvider _roomsProvider;
        [SerializeField] private PromocodesDatabase _promocodesDatabase;

        [FormerlySerializedAs("_gameInventory")]
        [Header("Inventory")]
        [SerializeField] private InventoryConfig _inventoryConfig;

        [SerializeField] private GlobalInventory _globalInventory;

        [Header("UserStats")]
        [SerializeField] private RanksDatabase _ranksDatabase;

        [SerializeField] private CostsDatabase _costsDatabase;

        [Header("Settings")]
        [SerializeField] private MouseSensivityConfig _mouseSensivityConfig;

        [Header("Audio")]
        [SerializeField] private SoundProvider _soundProvider;

        [SerializeField] private AudioMixer _audioMixer;

        [Header("Other")]
        [SerializeField] private TaskProviderConfig _tasksProviderConfig;

        public override void InstallBindings()
        {
            BindGlobalInitialState();
            BindStorage();
            BindFileProvider();

            BindParser();
            BindLocalizationDataProvider();
            BindLocalizationSystem();
            BindSettings();

            Container.Bind<RoomsProvider>().FromInstance(_roomsProvider);
            
            Container.Bind<ISceneTransition>().FromInstance(_sceneTransition.Value);
            Container.Bind<ConnectionProvider>().FromInstance(_connectionProvider);

            Container.Bind<ICommandsProvider>().To<CommandsProvider>().AsSingle();
            Container.Bind<MoveToStateCommand>().AsTransient();

            Container.Bind<AdvertisementProvider>().AsSingle();

            BindPromoCodesServices();
            BindUserStatsServices();

            Container.Bind<MapsProvider>().AsSingle();
            Container.Bind<GameModProvider>().AsSingle().WithArguments(_gameModsConfig);

            BindTasks();
        }

        private void BindTasks()
        {
            Container.BindInterfacesAndSelfTo<TasksProvider>().AsSingle().WithArguments(_tasksProviderConfig);
            BindTasksDataProvider();
        }

        private void BindTasksDataProvider()
        {
#if YandexGamesPlatform_yg
            Container.Bind<IDataProvider<TasksData>>()
                .To<YandexTasksDataProvider>()
                .AsSingle();
#else
            Container.Bind<IDataProvider<TasksData>>()
                .To<DataProvider<TasksData>>()
                .AsSingle()
                .WithArguments("TasksDataSavesKey");
#endif
        }

        private void BindUserStatsServices()
        {
            BindUserStatsDataProvider();
            Container.Bind<RewardService>().AsSingle().WithArguments(_costsDatabase);
            Container.Bind<UserStatsProvider>().AsSingle().WithArguments(_inventoryConfig);
            Container.Bind<UserRankProvider>().AsSingle().WithArguments(_ranksDatabase);
            Container.Bind<CoinsProvider>().AsSingle();
            Container.Bind<TicketsProvider>().AsSingle();
            Container.Bind<InventoryProvider>().AsSingle().WithArguments(_globalInventory);
        }

        private void BindPromoCodesServices()
        {
#if YandexGamesPlatform_yg
            Container.Bind<IDataProvider<PromocodesSavesData>>().To<YandexPromocodesDataProvider>().AsSingle();
#else
            Container.Bind<IDataProvider<PromocodesSavesData>>()
                .To<DataProvider<PromocodesSavesData>>()
                .AsSingle().WithArguments("PromoCodesSavesKey");
#endif
            Container.Bind<PromocodesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PromoCodesProvider>().AsSingle().WithArguments(_promocodesDatabase)
                .NonLazy();
        }

        private void BindSettings()
        {
            Container.Bind<MouseSensivityProvider>().AsSingle().WithArguments(_mouseSensivityConfig);
            Container.Bind<ISoundProvider>().FromInstance(_soundProvider).AsSingle();

            Container.Bind<IAudioService>().To<AudioService>().AsSingle().WithArguments(_audioMixer);
            Container.BindInterfacesAndSelfTo<SettingsProvider>().AsSingle();
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
            BindSettingsDataProvider();
        }

        private void BindSettingsDataProvider()
        {
#if YandexGamesPlatform_yg
            Container
                .Bind<IDataProvider<SettingsData>>()
                .To<YandexSettingsDataProvider>()
                .AsSingle();
#else
            Container
                .Bind<IDataProvider<SettingsData>>()
                .To<DataProvider<SettingsData>>()
                .AsSingle()
                .WithArguments("settingsSaves");
#endif
        }

        private void BindUserStatsDataProvider()
        {
#if YandexGamesPlatform_yg
            Container
                .Bind<IDataProvider<UserStatsData>>()
                .To<YandexUserStatsDataProvider>()
                .AsSingle();
#else
            Container
                .Bind<IDataProvider<UserStatsData>>()
                .To<DataProvider<UserStatsData>>()
                .AsSingle()
                .WithArguments("userStatsDataSaves");
#endif
        }

        private void BindLocalizationSystem()
        {
#if YandexGamesPlatform_yg
                var lang = YG2.lang.Equals("ru") ? "ru" : "en";
#else
                var lang = _language;
#endif
            Container.Bind<LocalizationDatabase>().FromInstance(_localizationDatabase);
            Container
                .Bind<ILocalizationSystem>()
                .To<LocalizationSystem>()
                .AsSingle()
                .WithArguments(lang);
        }

        private void BindLocalizationDataProvider()
        {
            Container
                .Bind<IDataProvider<LocalizationSavesData>>()
                .To<DataProvider<LocalizationSavesData>>()
                .AsSingle()
                .WithArguments(LocalizationDataKey.KEY);
        }

        private void BindParser()
        {
            Container.Bind<IParser>().To<CSVParser>().AsSingle();
        }

        private void BindFileProvider()
        {
            Container.Bind<IFileProvider>().To<ResourcesFileProvider>().AsSingle();
        }

        private void BindStorage()
        {
            Container.Bind<IStorage>().To<PlayerPrefsStorage>().AsSingle();
        }

        private void BindGlobalInitialState()
        {
            Container
                .Bind<State>()
                .To<GlobalInitialState>()
                .AsSingle();
        }
    }
}