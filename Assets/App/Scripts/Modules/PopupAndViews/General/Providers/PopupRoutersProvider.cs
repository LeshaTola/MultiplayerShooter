using Zenject;

namespace App.Scripts.Modules.PopupAndViews.General.Providers
{
    public class PopupRoutersProvider
    {
        private readonly DiContainer _container;

        public PopupRoutersProvider(DiContainer container)
        {
            _container = container;
        }

        public T GetPopupRouter<T>() where T : IPopupRouter
        {
            return _container.Resolve<T>();
        }
    }

    public interface IPopupRouter
    {
        
    }
}