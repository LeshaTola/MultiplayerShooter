#if GOOGLE
using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
#endif
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms.YandexMobile
{
    public class AppodealMobileAdProvider : MonoBehaviour
    {
#if GOOGLE

        #region Appodeal Application Key

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IOS
        private const string DEFAULT_APP_KEY = "";
#elif UNITY_ANDROID
        private const string DEFAULT_APP_KEY = "b53393e0c1a4184c07c231c46490c9a7423a75a269ae2f9f";
#elif UNITY_IOS
        private const string DEFAULT_APP_KEY = "";
#else
        private const string DEFAULT_APP_KEY = "";
#endif
        private static string AppKey => PlayerPrefs.GetString(AppodealConstants.AppKeyPlayerPrefsKey, DEFAULT_APP_KEY);

        #endregion

        public event Action<string> OnRewardedSuccess;

        private string _rewardedAdId;
        private Action _rewardedAdAction;

        public static AppodealMobileAdProvider Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;

            Initialize();
        }

        private void Initialize()
        {
            InitAppodeal();
        }

        private void InitAppodeal()
        {
            Appodeal.SetCustomFilter(PredefinedKeys.UserAge, 18);
            Appodeal.SetCustomFilter(PredefinedKeys.UserGender, (int) AppodealUserGender.Female);

            Appodeal.SetLocationTracking(false);
            Appodeal.MuteVideosIfCallsMuted(true);
            Appodeal.SetChildDirectedTreatment(false);

            Appodeal.SetTriggerOnLoadedOnPrecache(AppodealAdType.Interstitial, true);

            Appodeal.DisableNetwork(AppodealNetworks.Vungle);
            Appodeal.DisableNetwork(AppodealNetworks.Yandex, AppodealAdType.Banner);

            Appodeal.SetAutoCache(AppodealAdType.Interstitial, true);
            Appodeal.SetAutoCache(AppodealAdType.RewardedVideo, true);

            SetAppodealCallbacks();

            int adTypes = (AppodealAdType.Interstitial) | (AppodealAdType.RewardedVideo);

            Appodeal.Initialize(AppKey, adTypes);
        }

        private void SetAppodealCallbacks()
        {
            AppodealCallbacks.RewardedVideo.OnLoaded += OnRewardedVideoLoaded;
            AppodealCallbacks.RewardedVideo.OnFailedToLoad += OnRewardedVideoFailedToLoad;
            AppodealCallbacks.RewardedVideo.OnShown += OnRewardedVideoShown;
            AppodealCallbacks.RewardedVideo.OnShowFailed += OnRewardedVideoShowFailed;
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
            AppodealCallbacks.RewardedVideo.OnClicked += OnRewardedVideoClicked;
            AppodealCallbacks.RewardedVideo.OnExpired += OnRewardedVideoExpired;
        }

        public void ShowInterstitial()
        {
            if (Appodeal.IsLoaded(AppodealAdType.Interstitial) &&
                Appodeal.CanShow(AppodealAdType.Interstitial, "default") &&
                !Appodeal.IsPrecache(AppodealAdType.Interstitial))
            {
                Appodeal.Show(AppodealShowStyle.Interstitial);
            }
            else if (!Appodeal.IsAutoCacheEnabled(AppodealAdType.Interstitial))
            {
                Appodeal.Cache(AppodealAdType.Interstitial);
            }
        }

        public void ShowRewardedVideo(string id)
        {
            _rewardedAdId = id;
            if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo) &&
                Appodeal.CanShow(AppodealAdType.RewardedVideo, "default"))
            {
                Appodeal.Show(AppodealShowStyle.RewardedVideo);
            }
            else if (!Appodeal.IsAutoCacheEnabled(AppodealAdType.RewardedVideo))
            {
                Appodeal.Cache(AppodealAdType.RewardedVideo);
            }
        }

        public void ShowRewardedVideo(string id, Action callback)
        {
            _rewardedAdAction = callback;
            ShowRewardedVideo(id);
        }

        #region InAppPurchaseValidation Callbacks

        private void OnInAppPurchaseValidationSucceeded(object sender, InAppPurchaseEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnInAppPurchaseValidationSucceeded(string json:\n{e.Json})");
        }

        private void OnInAppPurchaseValidationFailed(object sender, InAppPurchaseEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnInAppPurchaseValidationFailed(string json:\n{e.Json})");
        }

        #endregion

        #region RewardedVideoAd Callbacks

        private void OnRewardedVideoLoaded(object sender, AdLoadedEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoLoaded(bool isPrecache:{e.IsPrecache})");
            Debug.Log($"[APDUnity] GetPredictedEcpm(): {Appodeal.GetPredictedEcpm(AppodealAdType.RewardedVideo)}");
        }

        private void OnRewardedVideoFailedToLoad(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoFailedToLoad()");
        }

        private void OnRewardedVideoShowFailed(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShowFailed()");
        }

        private void OnRewardedVideoShown(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShown()");
            OnRewardedSuccess?.Invoke(_rewardedAdId);
            _rewardedAdAction?.Invoke();
            _rewardedAdAction = null;
        }

        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoClosed(bool finished:{e.Finished})");
        }

        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs e)
        {
            Debug.Log(
                $"[APDUnity] [Callback] OnRewardedVideoFinished(double amount:{e.Amount}, string name:{e.Currency})");
        }

        private void OnRewardedVideoExpired(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoExpired()");
        }

        private void OnRewardedVideoClicked(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoClicked()");
        }

        #endregion

#endif
    }
}