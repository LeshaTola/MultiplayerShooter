using App.Scripts.Features.Commands;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using App.Scripts.Features.SceneTransitions;
using App.Scripts.Features.Screens.Providers;
using App.Scripts.Features.Settings;
using App.Scripts.Features.StateMachines.States;
using App.Scripts.Features.UserStats;
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
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using TNRD;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace App.Scripts.Features.Bootstrap
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private LocalizationDatabase _localizationDatabase;
        [SerializeField] private string _language;

        [SerializeField] private SerializableInterface<ISceneTransition> _sceneTransition;

        [SerializeField] private ConnectionProvider _connectionProvider;

        [Header("Inventory")]
        [SerializeField] private GameInventory _gameInventory;
        [SerializeField] private GlobalInventory _globalInventory;

        [Header("UserStats")]
        [SerializeField] private RanksDatabase _ranksDatabase;
        
        [Header("Settings")]
        [SerializeField] private MouseSensivityConfig _mouseSensivityConfig;

        [Header("Audio")]
        [SerializeField] private SoundProvider _soundProvider;

        [SerializeField] private AudioMixer _audioMixer;

        public override void InstallBindings()
        {
            BindGlobalInitialState();
            BindStorage();
            BindFileProvider();

            BindParser();
            BindLocalizationDataProvider();
            BindLocalizationSystem();

            Container.Bind<MouseSensivityProvider>().AsSingle().WithArguments(_mouseSensivityConfig);
            Container.Bind<ISoundProvider>().FromInstance(_soundProvider).AsSingle();

            Container.Bind<IAudioService>().To<AudioService>().AsSingle().WithArguments(_audioMixer);
            Container.BindInterfacesAndSelfTo<SettingsProvider>().AsSingle();
            Container
                .Bind<IDataProvider<SettingsData>>()
                .To<DataProvider<SettingsData>>()
                .AsSingle()
                .WithArguments("settingsSaves");

            Container.Bind<IScreenService>().To<ScreenService>().AsSingle();

            Container.Bind<ISceneTransition>().FromInstance(_sceneTransition.Value);
            Container.Bind<ConnectionProvider>().FromInstance(_connectionProvider);

            Container.Bind<ICommandsProvider>().To<CommandsProvider>().AsSingle();
            Container.Bind<MoveToStateCommand>().AsTransient();

            Container.Bind<PresentersProvider>().AsSingle();
            Container.Bind<GameInputProvider>().AsSingle();
            
            Container.Bind<RewardService>().AsSingle();
            Container.Bind<UserRankProvider>().AsSingle().WithArguments(_ranksDatabase);
            
            Container.Bind<InventoryProvider>().AsSingle().WithArguments(_gameInventory, _globalInventory);
        }

        private void BindLocalizationSystem()
        {
            Container.Bind<LocalizationDatabase>().FromInstance(_localizationDatabase);
            Container
                .Bind<ILocalizationSystem>()
                .To<LocalizationSystem>()
                .AsSingle()
                .WithArguments(_language);
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