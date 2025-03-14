using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Strategies;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes.Factories
{
    public class PromocodesFactory
    {
        private DiContainer _container;

        public PromocodesFactory(DiContainer container)
        {
            _container = container;
        }

        public PromocodeAction GetPromocodeAction(PromocodeAction original)
        {
            var promocodeAction = (PromocodeAction)_container.Instantiate(original.GetType());
            promocodeAction.Import(original);
            return promocodeAction;
        }
    }
}