using System.Runtime.InteropServices;

namespace App.Scripts.Modules.OtherPlatforms.Ok
{
    public static class OKMarketing
    {
#if OK
        [DllImport("__Internal")]
        public static extern void OK_ShowAdsInterstitial();    
        
        [DllImport("__Internal")]
        public static extern void OK_ShowRewardAd(string id);   
        
        [DllImport("__Internal")]
        public static extern void SetKey_LocalStorage_js(string key, string json);   
        
        [DllImport("__Internal")]
        public static extern string GetKey_LocalStorage_js(string key);
#elif VK
        [DllImport("__Internal")]
        public static extern void ShowAdsInterstitial();    
        
        [DllImport("__Internal")]
        public static extern void ShowAdsReward(string id);   

#endif
    }
}