using YG;

namespace App.Scripts.Features.Yandex.Advertisement
{
    public class AdvertisementProvider
    {
        public bool IsCanShowAd { get; set; }

        public AdvertisementProvider()
        {
            IsCanShowAd = YG2.saves.IsCanShowAd;
        }

        public void ShowInterstitialAd()
        {
            if (IsCanShowAd)
            {
                YG2.InterstitialAdvShow();
            }
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public bool IsCanShowAd = true;
    }
}