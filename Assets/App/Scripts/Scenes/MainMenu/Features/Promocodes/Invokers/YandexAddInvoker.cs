using YG;

namespace App.Scripts.Scenes.MainMenu.Features.Promocodes
{
    public class YandexAddInvoker : PromocodeInvoker
    {
        protected override void OnEnable()
        {
            _button.onClick.AddListener(() => YG2.RewardedAdvShow(_promocode));
        }
    }
}