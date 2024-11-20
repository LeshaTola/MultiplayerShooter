using Zenject;

namespace App.Scripts.Modules.Factories.DiFactories
{
    public class ResolveFactory<T>:IFactory<T>
    {
        private DiContainer diContainer;

        public ResolveFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public T GetItem()
        {
            return diContainer.Resolve<T>();
        }
    }
}