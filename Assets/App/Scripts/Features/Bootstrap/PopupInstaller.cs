using App.Scripts.Modules.PopupAndViews.Configs;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Modules.PopupAndViews.General.Providers;
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
            BindPopupProvider();
            BindPopupController();
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