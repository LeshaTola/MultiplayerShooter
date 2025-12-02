using App.Scripts.Features;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.Sounds.Providers;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.Commands
{
    public class BuyItemCommand : LabeledCommand
    {
        private readonly InfoPopupRouter _infoPopupRouter;
        private readonly MarketService _marketService;
        private readonly ISoundProvider _soundProvider;
        private readonly string _buySound;
        public ShopItemData ItemData { get; set; }
        
        public BuyItemCommand(string label,
            InfoPopupRouter infoPopupRouter,
            MarketService marketService,
            ISoundProvider soundProvider,
            string buySound) : base(label)
        {
            _infoPopupRouter = infoPopupRouter;
            _marketService = marketService;
            _soundProvider = soundProvider;
            _buySound = buySound;
        }

        public override void Execute()
        {
            Buy();
        }

        private void Buy()
        {
            if (!_marketService.TryBuyItem(ItemData))
            {
                ShowNotEnoughMoneyMessage();
                return;
            }
            
            _soundProvider.PlayOneShotSound(_buySound);
        }

        private void ShowNotEnoughMoneyMessage()
        {
            _infoPopupRouter.ShowPopup(ConstStrings.ATTENTION, ConstStrings.NOT_ENOUGH_MONEY).Forget();
        }
    }
}