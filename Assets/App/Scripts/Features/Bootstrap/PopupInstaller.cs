using App.Scripts.Modules.PopupAndViews.Configs;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.PopupAndViews.General.Providers;
using App.Scripts.Modules.PopupAndViews.Popups.Image;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Features.Bootstrap
{
    public class PopupInstaller : MonoInstaller
    {
        [SerializeField] private Image screenBlocker;
        [SerializeField] private PopupDatabase popupDatabase;
        [SerializeField] private Transform container;

        public override void InstallBindings()
        {
            BindRouters();
            BindPopupProvider();
            BindPopupController();
        }

        private void BindRouters()
        {
            Container.Bind<RewardsPopupRouter>().AsSingle();
            Container.Bind<InfoPopupRouter>().AsSingle();
            Container.Bind<ImagePopupRouter>().AsSingle();
            Container.Bind<InputFieldPopupRouter>().AsSingle();
        }

        private void BindPopupController()
        {
            Container
                .Bind<IPopupController>()
                .To<PopupController>()
                .AsSingle()
                .WithArguments(screenBlocker);
        }

        private void BindPopupProvider()
        {
            Container
                .Bind<IPopupProvider>()
                .To<PopupProvider>()
                .AsSingle()
                .WithArguments(popupDatabase, container);
        }
    }
}