using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Scenes.Gameplay.Chat;
using App.Scripts.Scenes.Gameplay.EndGame;
using App.Scripts.Scenes.Gameplay.Esc;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.Gameplay.HUD.PlayerUI.Provider;
using App.Scripts.Scenes.Gameplay.Inventory;
using App.Scripts.Scenes.Gameplay.KillChat;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen.DailyTasks;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Bootstrap
{
    public class ViewsInstaller : MonoInstaller
    {

        [SerializeField] private PlayerUIProvider _playerUIProvider;
        
        [SerializeField] private LeaderBoardView _leaderBoardView;
        [SerializeField] private KillChatView _killChatView;
        [SerializeField] private ChatView _chatView;

        [Header("Respawn")]
        [SerializeField] private RespawnView _respawnView;
        
        [Header("ESC")]
        [SerializeField] private EscMenuView _escMenuView;
        [SerializeField] private SettingsView _settingsView;
        
        [Header("EndGame")]
        [SerializeField] private EndGameView _endGameView;

        [Header("Other")]
        [SerializeField] private RectTransform _overlayContainer;
        [SerializeField] private MobileInputView _mobileInputView;
        [SerializeField] private DailyTasksView _dailyTasksView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerUIProvider>().FromInstance(_playerUIProvider).AsSingle();
            
            Container.Bind<LeaderBoardView>().FromInstance(_leaderBoardView).AsSingle();
            
            Container.Bind<RespawnView>().FromInstance(_respawnView).AsSingle();
            
            Container.Bind<EscMenuView>().FromInstance(_escMenuView).AsSingle();
            Container.Bind<SettingsView>().FromInstance(_settingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<EscScreenPresenter>().AsSingle();
            
            Container.Bind<EndGameView>().FromInstance(_endGameView).AsSingle();
            Container.BindInterfacesAndSelfTo<EndGameViewPresenter>().AsSingle();
            
            Container.Bind<KillChatView>().FromInstance(_killChatView).AsSingle();
            Container.Bind<ChatView>().FromInstance(_chatView).AsSingle();
            Container.BindInterfacesAndSelfTo<ChatViewPresenter>().AsSingle();

            Container.Bind<SelectionProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInventoryViewPresenter>()
                .AsSingle()
                .WithArguments(_overlayContainer);

            Container.BindInstance(_mobileInputView).AsSingle();
            
            Container.BindInstance(_dailyTasksView).AsSingle();
            Container.BindInterfacesAndSelfTo<DailyTaskViewPresenter>().AsSingle();
        }
    }
}