using App.Scripts.Features.Inventory;
using App.Scripts.Scenes.Gameplay.EndGame;
using App.Scripts.Scenes.Gameplay.Esc;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.Gameplay.KillChat;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.MainMenu.Inventory.GameInventory;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Bootstrap
{
    public class ViewsInstaller:MonoInstaller
    {
        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private WeaponView _weaponView;
        [SerializeField] private LeaderBoardView _leaderBoardView;
        [SerializeField] private KillChatView _killChatView;

        [Header("Respawn")]
        [SerializeField] private RespawnView _respawnView;
        
        [Header("ESC")]
        [SerializeField] private EscMenuView _escMenuView;
        [SerializeField] private SettingsView _settingsView;
        
        [Header("Inventory")]
        [SerializeField] private GameInventoryView _view;
        [SerializeField] private RectTransform _overlayContainer;
        
        [Header("EndGame")]
        [SerializeField] private EndGameView _endGameView;

        public override void InstallBindings()
        {
            Container.Bind<HealthBarUI>().FromInstance(_healthBarUI).AsSingle();
            Container.Bind<WeaponView>().FromInstance(_weaponView).AsSingle();
            
            Container.Bind<LeaderBoardView>().FromInstance(_leaderBoardView).AsSingle();
            
            Container.Bind<RespawnView>().FromInstance(_respawnView).AsSingle();
            
            Container.Bind<EscMenuView>().FromInstance(_escMenuView).AsSingle();
            Container.Bind<SettingsView>().FromInstance(_settingsView).AsSingle();
            Container.BindInterfacesAndSelfTo<EscScreenPresenter>().AsSingle();
            
            Container.Bind<EndGameView>().FromInstance(_endGameView).AsSingle();
            Container.BindInterfacesAndSelfTo<EndGameViewPresenter>().AsSingle();
            
            Container.Bind<KillChatView>().FromInstance(_killChatView).AsSingle();
            
            Container.Bind<GameInventoryView>().FromInstance(_view).AsSingle();
            Container.BindInterfacesAndSelfTo<GameInventoryViewPresenter>().AsSingle().WithArguments(_overlayContainer);
        }
    }
}