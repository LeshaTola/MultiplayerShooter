using App.Scripts.Scenes.MainMenu.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private MainScreen _mainScreen;
        [SerializeField] private RoomsView _roomsView;

        public override void InstallBindings()
        {
            Container.Bind<MainScreenPresenter>().AsSingle();
            Container.BindInstance(_mainScreen).AsSingle();
            
            Container.Bind<RoomsViewPresenter>().AsSingle();
            Container.BindInstance(_roomsView).AsSingle();
        }
    }
}