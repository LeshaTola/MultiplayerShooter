using UnityEngine;
#if RUSTORE || HUAWEI
using System;
using YandexMobileAds;
using YandexMobileAds.Base;
#endif

namespace App.Scripts.Modules.OtherPlatforms.YandexMobile
{
    public class MobileAdProvider : MonoBehaviour
    {
#if RUSTORE || HUAWEI
#if RUSTORE || UNITY_EDITOR || UNITY_WEBGL
        private const string BANNER_BLOCK_ID = "demo-banner-yandex";
        private const string INTERSTITIAL_BLOCK_ID = "demo-interstitial-yandex";
        private const string REWARDED_BLOCK_ID = "demo-rewarded-yandex";

#elif HUAWEI
        private const string BANNER_BLOCK_ID = "demo-banner-yandex";
        private const string INTERSTITIAL_BLOCK_ID = "demo-interstitial-yandex";
        private const string REWARDED_BLOCK_ID = "demo-rewarded-yandex";
#elif GOOGLE
     private const string BANNER_BLOCK_ID = "demo-banner-yandex";
    private const string INTERSTITIAL_BLOCK_ID = "demo-interstitial-yandex";
     private const string REWARDED_BLOCK_ID = "demo-rewarded-yandex";
#endif

        public event Action<string> OnRewardedSuccess;

        private RewardedAdLoader _rewardedAdLoader;
        private RewardedAd _rewardedAd;
        private Action _rewardedAdAction;
        private string _rewardedAdId;

        private Banner _banner;

        private InterstitialAdLoader _interstitialAdLoader;
        private Interstitial _interstitial;

        public static MobileAdProvider Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
        }
        
        private void Start()
        {
            //Sets COPPA restriction for user age under 13
            MobileAds.SetAgeRestrictedUser(false);

            // SetBanner(true);

            SetupInterstitialLoader();
            RequestInterstitialAd();

            SetupRewardedLoader();
            RequestRewardedAd();
        }

        public void ShowInterstitial()
        {
            if (_interstitial != null)
            {
                _interstitial.Show();
            }
        }

        public void ShowRewardedVideo(string id)
        {
            _rewardedAdId = id;
            ShowRewardedAd();
        }

        public void ShowRewardedVideo(string id, Action callback)
        {
            _rewardedAdAction = callback;
            ShowRewardedVideo(id);
        }

        public void SetBanner(bool isActive)
        {
            string adUnitId = BANNER_BLOCK_ID;
            BannerAdSize bannerMaxSize = BannerAdSize.StickySize(GetScreenWidthDp());
            _banner = new Banner(adUnitId, bannerMaxSize, AdPosition.BottomCenter);

            AdRequest request = new AdRequest.Builder().Build();
            _banner.LoadAd(request);
            _banner.OnAdLoaded += OnBannerAdLoaded;
        }

        private void OnBannerAdLoaded(object sender, EventArgs e)
        {
            _banner.Show();
        }

        private int GetScreenWidthDp()
        {
            int screenWidth = (int) Screen.safeArea.width;
            return ScreenUtils.ConvertPixelsToDp(screenWidth);
        }

        private void SetupInterstitialLoader()
        {
            _interstitialAdLoader = new InterstitialAdLoader();
            _interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
            _interstitialAdLoader.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        }

        private void RequestInterstitialAd()
        {
            string adUnitId = INTERSTITIAL_BLOCK_ID;
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            _interstitialAdLoader.LoadAd(adRequestConfiguration);
        }

        private void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            _interstitial = args.Interstitial;
            _interstitial.OnAdDismissed += OnInterstitialDismissed;
            _interstitial.OnAdFailedToShow += OnInterstitialDismissed;
        }

        private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.LogError(args.Message);
        }

        private void OnInterstitialDismissed(object sender, EventArgs e)
        {
            DestroyInterstitialAd();
            RequestInterstitialAd();
        }

        private void DestroyInterstitialAd()
        {
            if (_interstitial != null)
            {
                _interstitial.Destroy();
                _interstitial = null;
            }
        }

        private void SetupRewardedLoader()
        {
            _rewardedAdLoader = new RewardedAdLoader();
            _rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
            _rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
        }

        private void RequestRewardedAd()
        {
            // Replace demo Unit ID 'demo-rewarded-yandex' with actual Ad Unit ID
            string adUnitId = REWARDED_BLOCK_ID;
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            _rewardedAdLoader.LoadAd(adRequestConfiguration);
        }


        private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            _rewardedAd = args.RewardedAd;
            _rewardedAd.OnAdFailedToShow += OnRewardedAdFailedToShow;
            _rewardedAd.OnAdDismissed += OnRewardedAdDismissed;
            _rewardedAd.OnRewarded += OnRewardedAdSuccess;
        }

        private void OnRewardedAdSuccess(object sender, Reward e)
        {
            OnRewardedSuccess?.Invoke(_rewardedAdId);
            _rewardedAdAction?.Invoke();
            _rewardedAdAction = null;
        }

        private void OnRewardedAdDismissed(object sender, EventArgs e)
        {
            DestroyRewardedAd();
            RequestRewardedAd();
        }

        private void OnRewardedAdFailedToShow(object sender, AdFailureEventArgs e)
        {
            DestroyRewardedAd();
            RequestRewardedAd();
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.LogError(args.Message);
        }
        
        private void ShowRewardedAd()
        {
            Debug.Log($"Showing rewarded {_rewardedAd}");
            if (_rewardedAd != null)
            {
                Debug.Log($"Rewarded");
                _rewardedAd.Show();
            }
        }

        public void DestroyRewardedAd()
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
        }
#endif
    }
}