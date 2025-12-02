using System;
using Newtonsoft.Json;
using UnityEngine;
using YG;

namespace App.Scripts.Modules.OtherPlatforms.Ok
{
    public class RewardService : MonoBehaviour
    {
        public static event Action<string> OnRewardSuccess;
        public static event Action RewardAction;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void OnInterstitialAdCompleted(string id)
        {
            Debug.Log($"Interstitial Ad Completed {id}");
        }
        
        public void OnRewardAdCompleted(string id)
        {
            Debug.Log("Reward ID: ----- " +id);
            if (!id.Contains("200"))
            {
                return;
            }
            id = id.Replace("200", "");
            OnRewardSuccess?.Invoke(id);
            RewardAction?.Invoke();
            RewardAction = null;
        }

        public void ToFocus(string id)
        {
            Debug.Log("FOCUS " + id);
        }


        public void ViewHide(string id)
        {
            Debug.Log("View hide - " + id);
            YG2.PauseGame(true);
        } 
        
        public void Restore(string id)
        {
            Debug.Log("View Restore - " + id);
            YG2.PauseGame(false);
        }
    }
}