using Zenject;

namespace App.Scripts.Features.Screens.Providers
{
    public class PresentersProvider
    {
        private DiContainer diContainer;

        public PresentersProvider(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public T GetPresenter<T>() where T : GameScreenPresenter
        {
            return (T) diContainer.Resolve(typeof(T));
        }
    }
}