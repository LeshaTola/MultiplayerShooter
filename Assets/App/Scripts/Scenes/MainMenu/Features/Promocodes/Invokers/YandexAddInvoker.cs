using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Invokers;
using YG;

namespace App.Scripts.Scenes.MainMenu.Features.PromoCodes
{
    public class YandexAddInvoker : PromocodeInvoker
    {
        protected override void OnEnable()
        {
            _button.onClick.AddListener(() => YG2.RewardedAdvShow(_promocode));
        }
    }
}